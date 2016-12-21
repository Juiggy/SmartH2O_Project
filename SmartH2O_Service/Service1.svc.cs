using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Globalization;

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static string strPathXMLSensorFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\param-data.xml";
        private static string strPathXMLAlarmFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\alarms-data.xml";
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public string WriteDataSensor(string strMsg)
        {
            /* 1º Verificar se a mensagem está valida com o schema
             * 2º Verificar se o ficheiro xml existe. não existe --> criar
             * 3º fazer append para esse ficheiro*/

            // 1º Verificar se a string está válida com o schema
            XmlSchemaSet schema = new XmlSchemaSet();
            try
            {
                
                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLSensorDataMsgSchema.xsd";
                schema.Add("", schemaPath);
            }
            catch(Exception e)
            {
                    return "Error with loading schema"; // erro com schema
            }
            bool validationErrors = false;
                XmlDocument xmlMsg = new XmlDocument();
                xmlMsg.LoadXml(strMsg);
                xmlMsg.Schemas.Add(schema);
                xmlMsg.Validate((s, eau) =>
                {
                    //Apresento mensagem de erro por o XML nao ser valido
                    validationErrors = true;
                });

            if (validationErrors)
                return "XML message not valid"; //mensagem xml nao valida
            else
            {
               // 2º Verificar se o ficheiro xml existe.não existe --> criar

                if (!XMLFileExists(strPathXMLSensorFile))
                {
                    //cria ficheiro
                    XmlDocument doc = new XmlDocument();
                    XmlElement parameters = doc.CreateElement("parameters");
                    doc.AppendChild(parameters);
                    doc.Save(strPathXMLSensorFile);
                }
                //tenho de fazer append dos dados
                XmlDocument docSensor = new XmlDocument();
                docSensor.Load(strPathXMLSensorFile);

                XmlNode root = docSensor.DocumentElement;
                

                root.AppendChild(docSensor.ImportNode(xmlMsg.DocumentElement,true));

                docSensor.Save(strPathXMLSensorFile);
            }


            return "0"; //0 para correr tudo bem
        }

        private bool XMLFileExists(string str)
        {
            if (System.IO.File.Exists(str))
                return true;
            else
                return false;
        }

        public string WriteDataAlarm(string strMsg)
        {
            /* 1º Verificar se a mensagem está valida com o schema
             * 2º Verificar se o ficheiro xml existe. não existe --> criar
             * 3º fazer append para esse ficheiro*/

            // 1º Verificar se a string está válida com o schema
            XmlSchemaSet schema = new XmlSchemaSet();
            try
            {

                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLAlarmDataMsgSchema.xsd";
                schema.Add("", schemaPath);
            }
            catch (Exception e)
            {
                return "Error with loading schema"; // erro com schema
            }
            bool validationErrors = false;
            XmlDocument xmlMsg = new XmlDocument();
            xmlMsg.LoadXml(strMsg);
            xmlMsg.Schemas.Add(schema);
            xmlMsg.Validate((s, eau) =>
            {
                //Apresento mensagem de erro por o XML nao ser valido
                validationErrors = true;
            });

            if (validationErrors)
                return "XML message not valid"; //mensagem xml nao valida
            else
            {
                // 2º Verificar se o ficheiro xml existe.não existe --> criar

                if (!XMLFileExists(strPathXMLAlarmFile))
                {
                    //cria ficheiro
                    XmlDocument doc = new XmlDocument();
                    XmlElement parameters = doc.CreateElement("alarms");
                    doc.AppendChild(parameters);
                    doc.Save(strPathXMLAlarmFile);
                }
                //tenho de fazer append dos dados
                XmlDocument docAlarm = new XmlDocument();
                docAlarm.Load(strPathXMLAlarmFile);

                XmlNode root = docAlarm.DocumentElement;


                root.AppendChild(docAlarm.ImportNode(xmlMsg.DocumentElement, true));

                docAlarm.Save(strPathXMLAlarmFile);
            }
            return "0"; //0 para correr tudo bem
        }


        public string getDataSensorXML()
        {
            XmlDocument doc = new XmlDocument();
            if (XMLFileExists(strPathXMLSensorFile))
            {
                doc.Load(strPathXMLSensorFile);
                return doc.OuterXml;
            }
            else
            {
                return "1001"; //erro -> documento nao existe
            }

        }
        public string getDataAlarmXML()
        {
            XmlDocument doc = new XmlDocument();
            if (XMLFileExists(strPathXMLAlarmFile))
            {
                doc.Load(strPathXMLAlarmFile);
                return doc.OuterXml;
            }
            else
            {
                return "1001"; //erro -> documento nao existe
            }
        }

        public List<string> getParameterMinRangeDay(string parameter, string dateBg,string dateEd)
        {
            XmlDocument xmlSensor = new XmlDocument();
            List<string> lista = new List<string>();
            if (XMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;

                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                float min;

                for (DateTime dataAux= dataIn; dataAux <= dataEnd; dataAux= dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + auxData + "']");

                    string aux;
                    float aux2;
                    min = float.MaxValue;
                    //chamo o metodo que me devolve o valor do dia meto numa lista para devolver no fim
                    foreach (XmlNode node in nodeList)
                    {
                        aux = node["value"].InnerText;
                        aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                        if (min >= aux2)
                            min = aux2;
                    }
                    //adiciono a uma lista
                    if (min == float.MaxValue)
                        min = 0;
                    lista.Add(min.ToString());
                }
                //devolvo a lista
                return lista;
            }
            else
            {
                return null; //erro -> documento nao existe
            }
        }
        public List<string> getParameterMaxRangeDay(string parameter, string dateBg, string dateEd)
        {
            XmlDocument xmlSensor = new XmlDocument();
            List<string> lista = new List<string>();
            if (XMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;

                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                float max;

                for (DateTime dataAux = dataIn; dataAux <= dataEnd; dataAux = dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + auxData + "']");

                    string aux;
                    float aux2;
                    max = float.MinValue;
                    foreach (XmlNode node in nodeList)
                    {
                        aux = node["value"].InnerText;
                        //aux.Replace(".", ",");
                        aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                        if (max <= aux2)
                            max = aux2;
                    }

                    if (max == float.MinValue)
                        max = 0;
                    lista.Add(max.ToString());
                }

                return lista;
            }
            else
            {
                return null; //erro -> documento nao existe
            }

        }
        public List<string> getParameterAvgRangeDay(string parameter, string dateBg, string dateEd)
        {
            XmlDocument xmlSensor = new XmlDocument();
            List<string> lista = new List<string>();
            if (XMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;

                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                float conta;
                float soma;

                for (DateTime dataAux = dataIn; dataAux <= dataEnd; dataAux = dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + auxData + "']");

                    string aux;
                    float aux2;
                    soma = 0;
                    conta = 0;
                    foreach (XmlNode node in nodeList)
                    {
                        aux = node["value"].InnerText;
                        //aux.Replace(".", ",");
                        aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                        soma += aux2;
                        conta++;
                    }
                    if (conta == 0)
                        conta = 1;
                    lista.Add((soma / conta).ToString());
                }

                return lista;
            }
            else
            {
                return null; //erro -> documento nao existe
            }

        }

        public string[] getParameterMinHourInDay(string parameter, string day)
        {
            XmlDocument xmlSensor = new XmlDocument();
            string[] resultados = new string[24];
            if (XMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;
                XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + day + "']");
                
                string aux;
                float aux2;
                float min;
                //faço um for de 00 a 24 horas
                for (int i = 0; i <= 23; i++)
                {
                    aux = "";
                    aux2 = 0;
                    min = float.MaxValue;
                    foreach (XmlNode node in nodeList)
                    {
                        string[] strHora= node["time"].InnerText.Split(':');
                        int hora = Int32.Parse(strHora[0]);
                        if (hora == i)
                        {
                            aux = node["value"].InnerText;
                            aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                            if (min >= aux2)
                                min = aux2;

                        }
                    }
                    //tenho de adicionar aqui o parametro numa lista de 0 a 24
                    if (min == float.MaxValue)
                            min = 0;
                    resultados[i] = min.ToString();
                }
                //return de uma lista/array
                return resultados;
            }
            else
            {
                return resultados; //erro -> documento nao existe
            }
        }

        public string[] getParameterMaxHourInDay(string parameter, string day)
        {
            XmlDocument xmlSensor = new XmlDocument();
            string[] resultados = new string[24];
            if (XMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;
                XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + day + "']");
                

                string aux;
                float aux2;
                float max;
                //faço um for de 00 a 24 horas
                for (int i = 0; i <= 23; i++)
                {
                    aux = "";
                    aux2 = 0;
                    max = float.MinValue;
                    foreach (XmlNode node in nodeList)
                    {
                        string[] strHora = node["time"].InnerText.Split(':');
                        int hora = Int32.Parse(strHora[0]);
                        if (hora == i)
                        {
                            aux = node["value"].InnerText;
                            aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                            if (max <= aux2)
                                max = aux2;

                        }
                    }
                    //tenho de adicionar aqui o parametro numa lista de 0 a 24
                    if (max == float.MinValue)
                        max = 0;
                    resultados[i] = max.ToString();
                }
                //return de uma lista/array
                return resultados;
            }
            else
            {
                return resultados; //erro -> documento nao existe
            }
        }


        public string[] getParameterAvgHourInDay(string parameter, string day)
        {
            XmlDocument xmlSensor = new XmlDocument();
            string[] resultados = new string[24];
            if (XMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;
                XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + day + "']");

                string aux;
                float aux2;
                float min;
                int conta;
                float soma;
                float result;
                //faço um for de 00 a 24 horas
                for (int i = 0; i <= 23; i++)
                {
                    aux = "";
                    aux2 = 0;
                    conta = 0;
                    soma = 0;
                    result = 0;
                    foreach (XmlNode node in nodeList)
                    {
                        string[] strHora = node["time"].InnerText.Split(':');
                        int hora = Int32.Parse(strHora[0]);
                        if (hora == i)
                        {
                            aux = node["value"].InnerText;
                            aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                            soma += aux2;
                            conta++;

                        }
                    }
                    //tenho de adicionar aqui o parametro numa lista de 0 a 24
                    if (conta != 0)
                        result = soma / conta;
                    resultados[i] = result.ToString();
                }
                //return de uma lista/array
                return resultados;
            }
            else
            {
                return resultados; //erro -> documento nao existe
            }
        }

        public List<string> getParameterMinWeekly(string parameter, int week)
        {
            //vou considerar que recebo o valor de uma semana em int
            //pretendo saber qual é a data inicial e data final da semana
            //chamo os métodos que já fiz para obter os valores entre datas

            //metodo adaptado do stackoverflow
            /*stackoverflow.com/questions/9592650/week-number-of-a-year-in-dateformat*/

            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int year = DateTime.Now.Year;
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)cul.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = cul.Calendar.GetWeekOfYear(jan1, cul.DateTimeFormat.CalendarWeekRule, cul.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                week -= 1;
            }
            firstWeekDay = firstWeekDay.AddDays(week * 7);
            string dateBg = firstWeekDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            firstWeekDay = firstWeekDay.AddDays(6);
            string dateEd = firstWeekDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return getParameterMinRangeDay(parameter, dateBg, dateEd);
        }
        public List<string> getParameterMaxWeekly(string parameter, int week)
        {
            //vou considerar que recebo o valor de uma semana em int
            //pretendo saber qual é a data inicial e data final da semana
            //chamo os métodos que já fiz para obter os valores entre datas

            //metodo adaptado do stackoverflow
            /*stackoverflow.com/questions/9592650/week-number-of-a-year-in-dateformat*/

            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int year = DateTime.Now.Year;
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)cul.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = cul.Calendar.GetWeekOfYear(jan1, cul.DateTimeFormat.CalendarWeekRule, cul.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                week -= 1;
            }
            firstWeekDay = firstWeekDay.AddDays(week * 7);
            string dateBg = firstWeekDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            firstWeekDay = firstWeekDay.AddDays(6);
            string dateEd = firstWeekDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return getParameterMaxRangeDay(parameter, dateBg, dateEd);
        }
        public List<string> getParameterAvgWeekly(string parameter, int week)
        {
            //vou considerar que recebo o valor de uma semana em int
            //pretendo saber qual é a data inicial e data final da semana
            //chamo os métodos que já fiz para obter os valores entre datas

            //metodo adaptado do stackoverflow
            /*stackoverflow.com/questions/9592650/week-number-of-a-year-in-dateformat*/

            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int year = DateTime.Now.Year;
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)cul.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = cul.Calendar.GetWeekOfYear(jan1, cul.DateTimeFormat.CalendarWeekRule, cul.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                week -= 1;
            }
            firstWeekDay = firstWeekDay.AddDays(week * 7);
            string dateBg = firstWeekDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            firstWeekDay = firstWeekDay.AddDays(6);
            string dateEd = firstWeekDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return getParameterAvgRangeDay(parameter, dateBg, dateEd);
        }

        public List<string> getAlarmRangeDay(List<string> parameters, string dateBg, string dateEd)
        {
            List<string> lista = new List<string>();
            XmlDocument xmlAlarm = new XmlDocument();

            if (XMLFileExists(strPathXMLAlarmFile))
            {
                xmlAlarm.Load(strPathXMLAlarmFile);
                XmlNode root = xmlAlarm.DocumentElement;
                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                for (DateTime dataAux = dataIn; dataAux <= dataEnd; dataAux = dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    foreach(string parameter in parameters)
                    {
                        XmlNodeList nodeList = root.SelectNodes("//alarm[type='" + parameter + "' and date='" + auxData + "']");
                        foreach (XmlNode node in nodeList)
                        {
                            //existe um alarme. tenho de adicionar uma string à lista para devolver no fim.
                            //vou fazer já formatado para na app ser só inserir na lista
                            string aux = node["time"].InnerText;
                            string[] auxTime=aux.Split('.');
                            aux = auxTime[0];
                            lista.Add("Sensor: " + node["idSensor"].InnerText + " has given an Alarm at: " + aux + " with message: " + node["message"].InnerText);

                        }
                    }
                   

                }

                return lista;
            }
            else
            {
                return null;
            }
                
        }


    }
}
