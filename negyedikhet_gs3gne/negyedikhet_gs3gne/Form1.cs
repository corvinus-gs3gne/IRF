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

        private int _million = (int)Math.Pow(10, 6);
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> Flats;
        Excel.Application xlApp;
        Excel.Workbook xlWBook;
        Excel.Worksheet xlWSheet;
        string[] headers;



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
                FormatTable();

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


        private void CreateTable()
        {
            headers = new string[]
           {
                "Kód",
                "Eladó",
                "Oldal",
                "Kerület",
                "Lift",
                "Szobák száma",
                "Alapterület (m2)",
                "Ár (mFt)",
                "Négyzetméter ár (Ft/m2)"



           };

            for (int i = 0; i < headers.Length; i++)
            {
                xlWSheet.Cells[1, i + 1] = headers[i];
            }

            object[,] values = new object[Flats.Count, headers.Length];

            int counter = 0;
            int floorColumn = 6;

            foreach (Flat f in Flats)
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;
                if (f.Elevator)
                {
                    values[counter, 4] = "Van";
                }
                else
                {
                    values[counter, 4] = "Nincs";
                }
                values[counter, 5] = f.NumberOfRooms;
                values[counter, floorColumn] = f.FloorArea;
                values[counter, 7] = f.Price;
                values[counter, 8] = String.Format("={0}/{1}*{2}",
                    "H" + (counter + 2).ToString(),
                    GetCell(counter + 2, floorColumn + 1),
                    _million.ToString()); //"H2/G2*1000000"

                counter++;
            }

            var range = xlWSheet.get_Range(
                GetCell(2, 1),
                GetCell(1 + values.GetLength(0), values.GetLength(1)));

            range.Value2 = values;
        }


        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }




        private void FormatTable()
        {
            Excel.Range headerRange = xlWSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            int LastRowID = xlWSheet.UsedRange.Rows.Count;
            Excel.Range CompleteTableRange = xlWSheet.get_Range(GetCell(1, 1), GetCell(LastRowID, headers.Length));
            CompleteTableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);


            Excel.Range firstColumnRange = xlWSheet.get_Range(GetCell(2, 1), GetCell(LastRowID, 1));
            firstColumnRange.Interior.Color = Color.LightYellow;
            firstColumnRange.Font.Bold = true;

            Excel.Range lastColumn = xlWSheet.get_Range(GetCell(2, headers.Length), GetCell(LastRowID, headers.Length));
            lastColumn.Interior.Color = Color.LightGreen;
            lastColumn.NumberFormat = "0.00";
        }


    }
}
