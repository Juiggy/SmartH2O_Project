using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace ExcelLib
{
    public class ExcelHandler
    {
        public static void CreateNewExcelFile(string path)
        {
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;
            var excelWorkbook = excelApplication.Workbooks.Add();
            
            excelWorkbook.SaveAs(path, AccessMode: Excel.XlSaveAsAccessMode.xlNoChange);
            excelWorkbook.Close();
            excelApplication.Quit();
            //It’s necessary to free all the memory used by the excel objects. e.g.:
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            excelWorkbook = null;
            //...
            GC.Collect();
        }
        public static void WriteToExcelFile(string path, List<string[]> listasMin, List<string[]> listasMax, List<string[]> listasAvg, string[] parametros)
        {

            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = false;
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(path);
            Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;



            //escrever dados para o excel
            int linha;
            int coluna = 600;
            int aux = 0;
            foreach (string[] lista in listasMin)
            {
                linha = 0;
                
                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(10, (300*aux)+10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;
                
                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }
                
                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Hours",
                 ValueTitle: "Min Values");
                coluna++;
                aux++;
            }
             aux = 0;
            coluna++;
            foreach (string[] lista in listasMax)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(320, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux=dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Hours",
                 ValueTitle: "Max Values");
                coluna++;
                aux++;
            }
            aux = 0;
            coluna++;
            foreach (string[] lista in listasAvg)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(640, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux=dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Hours",
                 ValueTitle: "Average Values");
                coluna++;
                aux++;
            }




            excelWorkbook.Save();
            excelWorkbook.Close();
            excelApplication.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            excelWorkbook = null;
            //Don’t forget to free the memory used by excel objects
            //...
            GC.Collect();
        }
        public static void WriteToExcelFileWeek(string path, List<string[]> listasMin, List<string[]> listasMax, List<string[]> listasAvg, string[] parametros)
        {

            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = false;
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(path);
            Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;



            //escrever dados para o excel
            int linha;
            int coluna = 600;
            int aux = 0;
            foreach (string[] lista in listasMin)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(10, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Day of Week",
                 ValueTitle: "Min Values");
                coluna++;
                aux++;
            }
            aux = 0;
            coluna++;
            foreach (string[] lista in listasMax)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(320, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Day of Week",
                 ValueTitle: "Max Values");
                coluna++;
                aux++;
            }
            aux = 0;
            coluna++;
            foreach (string[] lista in listasAvg)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(640, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Day of Week",
                 ValueTitle: "Average Values");
                coluna++;
                aux++;
            }
            excelWorkbook.Save();
            excelWorkbook.Close();
            excelApplication.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            excelWorkbook = null;
            //Don’t forget to free the memory used by excel objects
            //...
            GC.Collect();
        }
        public static void WriteToExcelFileRangeDays(string path, List<string[]> listasMin, List<string[]> listasMax, List<string[]> listasAvg, string[] parametros)
        {

            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = false;
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(path);
            Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;



            //escrever dados para o excel
            int linha;
            int coluna = 600;
            int aux = 0;
            foreach (string[] lista in listasMin)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(10, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Days",
                 ValueTitle: "Min Values");
                coluna++;
                aux++;
            }
            aux = 0;
            coluna++;
            foreach (string[] lista in listasMax)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(320, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Days",
                 ValueTitle: "Max Values");
                coluna++;
                aux++;
            }
            aux = 0;
            coluna++;
            foreach (string[] lista in listasAvg)
            {
                linha = 0;

                Excel.Chart myChart = null;
                Excel.ChartObjects charts = excelWorksheet.ChartObjects();
                Excel.ChartObject chartObject = charts.Add(640, (300 * aux) + 10, 300, 300); //left, top, width, heigh
                myChart = chartObject.Chart;

                foreach (string dado in lista)
                {
                    linha++;
                    string dadoaux = dado.Replace(",", ".");
                    excelWorksheet.Cells[linha, coluna].Value = dadoaux;
                }

                //set chart range            
                Excel.Range c1 = excelWorksheet.Cells[1, coluna];
                Excel.Range c2 = excelWorksheet.Cells[linha, coluna];
                Excel.Range myrange = excelWorksheet.get_Range(c1, c2);
                myChart.SetSourceData(myrange);
                //chart properties using the named parameters and default parameters functionality in the .NET
                myChart.ChartType = Excel.XlChartType.xlLine;
                myChart.ChartWizard(Source: myrange,
                 Title: parametros[aux],
                 CategoryTitle: "Days",
                 ValueTitle: "Average Values");
                coluna++;
                aux++;
            }
            excelWorkbook.Save();
            excelWorkbook.Close();
            excelApplication.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            excelWorkbook = null;
            //Don’t forget to free the memory used by excel objects
            //...
            GC.Collect();
        }
    }
}
