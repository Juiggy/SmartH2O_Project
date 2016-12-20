using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Schema;
using System.Xml;

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
    }


}
