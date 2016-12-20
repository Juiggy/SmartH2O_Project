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

                if (!sensorXMLFileExists(strPathXMLSensorFile))
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

        private bool sensorXMLFileExists(string str)
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

                if (!sensorXMLFileExists(strPathXMLAlarmFile))
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
            if (sensorXMLFileExists(strPathXMLSensorFile))
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
            if (sensorXMLFileExists(strPathXMLAlarmFile))
            {
                doc.Load(strPathXMLAlarmFile);
                return doc.OuterXml;
            }
            else
            {
                return "1001"; //erro -> documento nao existe
            }
        }

        public string getParameterMinRangeDay(string parameter, string dateBg,string dateEd)
        {
            XmlDocument xmlSensor = new XmlDocument();
            if (sensorXMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;

                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                float min = float.MaxValue;

                for (DateTime dataAux= dataIn; dataAux <= dataEnd; dataAux= dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + auxData + "']");

                    string aux;
                    float aux2;
                    foreach (XmlNode node in nodeList)
                    {
                        aux = node["value"].InnerText;
                        //aux.Replace(".", ",");
                        aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                        if (min >= aux2)
                            min = aux2;
                    }
                }

                return min.ToString();
            }
            else
            {
                return "ERROR_NO_FILE"; //erro -> documento nao existe
            }
        }
        public string getParameterMaxRangeDay(string parameter, string dateBg, string dateEd)
        {
            XmlDocument xmlSensor = new XmlDocument();
            if (sensorXMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;

                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                float max = float.MinValue;

                for (DateTime dataAux = dataIn; dataAux <= dataEnd; dataAux = dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + auxData + "']");

                    string aux;
                    float aux2;
                    foreach (XmlNode node in nodeList)
                    {
                        aux = node["value"].InnerText;
                        //aux.Replace(".", ",");
                        aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                        if (max <= aux2)
                            max = aux2;
                    }
                }

                return max.ToString();
            }
            else
            {
                return "ERROR_NO_FILE"; //erro -> documento nao existe
            }

        }
        public string getParameterAvgRangeDay(string parameter, string dateBg, string dateEd)
        {
            XmlDocument xmlSensor = new XmlDocument();
            if (sensorXMLFileExists(strPathXMLSensorFile))
            {
                xmlSensor.Load(strPathXMLSensorFile);

                //Quero os nós do parametro e do dia em especifico
                //faço um ciclo for each para verificar o que quero
                XmlNode root = xmlSensor.DocumentElement;

                DateTime dataIn = DateTime.Parse(dateBg);
                DateTime dataEnd = DateTime.Parse(dateEd);
                float conta = 0;
                float soma = 0;

                for (DateTime dataAux = dataIn; dataAux <= dataEnd; dataAux = dataAux.AddDays(1))
                {
                    string auxData = dataAux.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    XmlNodeList nodeList = root.SelectNodes("//parameter[type='" + parameter + "' and date='" + auxData + "']");

                    string aux;
                    float aux2;
                    foreach (XmlNode node in nodeList)
                    {
                        aux = node["value"].InnerText;
                        //aux.Replace(".", ",");
                        aux2 = float.Parse(aux, CultureInfo.InvariantCulture);
                        soma += aux2;
                        conta++;
                    }
                }

                return (soma/conta).ToString();
            }
            else
            {
                return "ERROR_NO_FILE"; //erro -> documento nao existe
            }

        }

        public string[] getParameterMinHourInDay(string parameter, string day)
        {
            XmlDocument xmlSensor = new XmlDocument();
            string[] resultados = new string[24];
            if (sensorXMLFileExists(strPathXMLSensorFile))
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
            if (sensorXMLFileExists(strPathXMLSensorFile))
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
            if (sensorXMLFileExists(strPathXMLSensorFile))
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

    }
}
