using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace negyedikhet_gs3gne
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> Flats;
        Excel.Application xlApp;
        Excel.Workbook xlWBook;
        Excel.Worksheet xlWSheet;
        

        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }

        private void LoadData()
        {
            Flats = context.Flats.ToList();

        }

        private void CreateExcel()
        {
            try
            {
                //Excel indítása, objektum betöltés

                xlApp = new Excel.Application();

                //új munkafüzet létrehoz

                xlWBook = xlApp.Workbooks.Add(Missing.Value);

                //új munkalap
                xlWSheet = xlWBook.ActiveSheet;

                //tábla létrehoz
                CreateTable();

                //control átadása felhasználónak

                xlApp.Visible = true;
                xlApp.UserControl = true;

            }
            catch (Exception ex) //hibakezelés beépített hibaüzivel
            {

                string errorMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errorMsg, "Error");

                //Excel app automat bezárása hiba esetén

                xlWBook.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWBook = null;
                xlApp = null;
            }
        }

        
    }
}
