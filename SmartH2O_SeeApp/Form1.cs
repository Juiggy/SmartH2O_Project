﻿using SmartH2O_Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelLib;

namespace SmartH2O_SeeApp
{
    public partial class Form1 : Form
    {
        static SmatH2O_Service.Service1Client serv;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rbSensor1.CheckedChanged += new EventHandler(rbSensor_CheckedChanged);
            rbSensor2.CheckedChanged += new EventHandler(rbSensor_CheckedChanged);
            rbSensor3.CheckedChanged += new EventHandler(rbSensor_CheckedChanged);
            rbAlarm1.CheckedChanged += new EventHandler(rbAlarm_CheckedChanged);
            rbAlarm2.CheckedChanged += new EventHandler(rbAlarm_CheckedChanged);
            for (int i = 0; i < clbParameters.Items.Count; i++)
            {
                clbParameters.SetItemChecked(i, true);
            }


            try
            {
                serv = new SmatH2O_Service.Service1Client();
            }
            catch(Exception eau)
            {
                //criar caixa de texto para sair com erro
                Environment.Exit(1);
            }
        }

        private void rbAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAlarm1.Checked)
            {
                dtAlarm1.Visible = true;
                dtAlarm2.Visible = false;
            }
            else
            if (rbAlarm2.Checked)
            {
                dtAlarm1.Visible = true;
                dtAlarm2.Visible = true;
            }
        }

        private void rbSensor_CheckedChanged(object sender, EventArgs e)
        {
            //evento para o caso de alterar as radio buttons dos sensores
            if (rbSensor1.Checked)
            {
                dtSensor1.Visible = true;
                dtSensor2.Visible = false;
                lbWeek.Visible = false;
            }
            else 
            if (rbSensor2.Checked)
            {
                dtSensor1.Visible = false;
                dtSensor2.Visible = false;
                lbWeek.Visible = true;
            }
            else
            if (rbSensor3.Checked)
            {
                dtSensor1.Visible = true;
                dtSensor2.Visible = true;
                lbWeek.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbSensor1.Checked)
            {
                
            }
            else
           if (rbSensor2.Checked)
            {
               
            }
            else
           if (rbSensor3.Checked)
            {
               
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btAlarm_Click(object sender, EventArgs e)
        {
            //preciso da lista de parametros
            string[] listaParametros = GetListaParametros();
            lbAlarme.Visible = true;
            
            if (rbAlarm1.Checked)
            {
                //lista de alarmes por 1 dia
                string date = dateToStr(dtAlarm1.Text);
                
                string[] alarmes = serv.getAlarmRangeDay(listaParametros, date, date);
                lbAlarme.Items.Clear();
                foreach (string alarme in alarmes)
                {
                    
                    lbAlarme.Items.Add(alarme);
                }
            }
            else
            if (rbAlarm2.Checked)
            {
                //lista de alarmes entre duas datas
                string date = dateToStr(dtAlarm1.Text);
                string dateFim = dateToStr(dtAlarm2.Text);
                string[] alarmes = serv.getAlarmRangeDay(listaParametros, date, dateFim);
                lbAlarme.Items.Clear();
                foreach (string alarme in alarmes)
                {
                    
                    lbAlarme.Items.Add(alarme);
                }
            }


        }

        private string[] GetListaParametros()
        {
            
            if (clbParameters.CheckedItems.Count != 0)
            {
                string[] lista = new string[clbParameters.CheckedItems.Count];
                // If so, loop through all checked items and print results.

                for (int x = 0; x <= clbParameters.CheckedItems.Count - 1; x++)
                {
                     lista[x]=(clbParameters.CheckedItems[x].ToString());
                }
                return lista;
            }
            else
            {
                string[] lista = new string[1];
                lista[0] = " ";
                return lista;
            }
        }

        private string dateToStr(string data)
        {
            DateTime date = DateTime.Parse(data);
            string aux = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return aux;
        }

        private void btSensor_Click(object sender, EventArgs e)
        {
            
            string[] parametros = GetListaParametros();
            List<string[]> listaMin = new List<string[]>();
            List<string[]> listaMax = new List<string[]>();
            List<string[]> listaAvg = new List<string[]>();
            if (rbSensor1.Checked)
            {
                ExcelHandler.CreateNewExcelFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\SensorGrafico.xlsx");

                foreach (string parametro in parametros)
                {
                    listaMin.Add(serv.getParameterMinHourInDay(parametro, dateToStr(dtSensor1.Text)));
                    listaMax.Add(serv.getParameterMaxHourInDay(parametro, dateToStr(dtSensor1.Text)));
                    listaAvg.Add(serv.getParameterAvgHourInDay(parametro, dateToStr(dtSensor1.Text)));
                }
                ExcelHandler.WriteToExcelFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\SensorGrafico.xlsx", listaMin, listaMax, listaAvg, parametros);
            }
            else
           if (rbSensor2.Checked)
            {
                //fazer uma verificação se o valor da caixa está entre 1 e 52
                
                if ((lbWeek.Value>0) && (lbWeek.Value < 53)) {
                    ExcelHandler.CreateNewExcelFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\SensorGrafico.xlsx");

                    foreach (string parametro in parametros)
                    {
                        int auxSemana = Decimal.ToInt32(lbWeek.Value);
                        listaMin.Add(serv.getParameterMinWeekly(parametro, auxSemana));
                        listaMax.Add(serv.getParameterMaxWeekly(parametro, auxSemana));
                        listaAvg.Add(serv.getParameterAvgWeekly(parametro, auxSemana));
                    }
                    ExcelHandler.WriteToExcelFileWeek(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\SensorGrafico.xlsx", listaMin, listaMax, listaAvg, parametros);
                }
                else
                {
                    MessageBox.Show("Week number out of range [1-52]");
                }
            }
            else
           if (rbSensor3.Checked)
            {
                ExcelHandler.CreateNewExcelFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\SensorGrafico.xlsx");
                foreach (string parametro in parametros)
                {
                    listaMin.Add(serv.getParameterMinRangeDay(parametro, dateToStr(dtSensor1.Text), dateToStr(dtSensor2.Text)));
                    listaMax.Add(serv.getParameterMaxRangeDay(parametro, dateToStr(dtSensor1.Text), dateToStr(dtSensor2.Text)));
                    listaAvg.Add(serv.getParameterAvgRangeDay(parametro, dateToStr(dtSensor1.Text), dateToStr(dtSensor2.Text)));
                }
                ExcelHandler.WriteToExcelFileRangeDays(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\SensorGrafico.xlsx", listaMin, listaMax, listaAvg, parametros);

            }


        }
    }
}
