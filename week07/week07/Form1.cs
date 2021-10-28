using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        string result;

        public Form1()
        {
            InitializeComponent();
            GetExchangeRates();
            XMLfeldolgozo();
            dataGridView1.DataSource = Rates;
            ShowDataOnChart();
            chartRateData.DataSource = Rates;

        }


        private void GetExchangeRates()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };

            var response = mnbService.GetExchangeRates(request);
            result = response.GetExchangeRatesResult;

        }

        private void XMLfeldolgozo()
        {

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                RateData rd = new RateData();
                Rates.Add(rd);

                rd.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                rd.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0) rd.Value = value / unit;
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


    }
}
