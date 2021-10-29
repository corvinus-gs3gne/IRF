using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using week07.Entities;
using week07.MnbServiceReference;

namespace week07
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        
        

        public Form1()
        {
            InitializeComponent();
            GetCurrencies();
            comboBox1.DataSource = Currencies;
            RefreshData();
             

        }


        private string GetExchangeRates()
        {
            

            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd")
            };

            var response = mnbService.GetExchangeRates(request);
            string result = response.GetExchangeRatesResult;
            File.WriteAllText("export.xml", result);
            return result;
        }

        private void XMLfeldolgozo(string input)
        {

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(input);
            foreach (XmlElement element in xml.DocumentElement)
            {
                RateData rd = new RateData();                

                rd.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null) continue;
                rd.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0) rd.Value = value / unit;
                Rates.Add(rd);
            }


        }

        private void ShowDataOnChart()
        {
            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.BorderWidth = 2;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
            
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

        }

        private void RefreshData()
        {
            Rates.Clear();
            if (comboBox1.SelectedItem == null) return;
            string xmlExRates= GetExchangeRates();
            
            XMLfeldolgozo(xmlExRates);
            ShowDataOnChart();
            
            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
            
        }

        

        private void GetCurrencies()
        {
            
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();      
                     
            var response = mnbService.GetCurrencies(request);
            string result = response.GetCurrenciesResult;
            File.WriteAllText("valutak", result);

            XmlDocument vxml = new XmlDocument();
            vxml.LoadXml(result);
            foreach (XmlElement element in vxml.DocumentElement.FirstChild.ChildNodes)
            {                            
                                
                Currencies.Add(element.InnerText);
            }

        }

        private void filterChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
