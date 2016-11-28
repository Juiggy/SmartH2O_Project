using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SoftwareOrganizationSmartH2O; //lib SoftwareOrganization
using uPLibrary.Networking.M2Mqtt; //lib para Mosquitto
using uPLibrary.Networking.M2Mqtt.Messages; //lib para Mosquitto
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Net;


namespace SmartH2O_Data_Uploader
{
    class Program
    {

        //nao houve erros, publico mensagem
        static MqttClient m_cClient; //= new MqttClient(SmartH2O_Data_Uploader.Properties.Settings.Default.BrookerIP);
        static XmlSchemaSet schema = new XmlSchemaSet();
        string[] m_strTopicsInfo = { "dataSensor" };
        static void Main(string[] args)
        {
            //valido o XML com o schema
            //schema = new XmlSchemaSet();

            //como fazer isto de forma mais dinamica?
            

            string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLSensorDataMsgSchema.xsd";
            schema.Add("", schemaPath);

            m_cClient = new MqttClient(SmartH2O_Data_Uploader.Properties.Settings.Default.BrookerIP);
            SensorNodeDll.SensorNodeDll dll = new SensorNodeDll.SensorNodeDll();
            int delay = SmartH2O_Data_Uploader.Properties.Settings.Default.sensorTime;
            
            try { 
            if (!m_cClient.IsConnected)
                m_cClient.MqttMsgPublished += m_cClient_MsgPublished;
                m_cClient.Connect(Guid.NewGuid().ToString());

                if (!m_cClient.IsConnected)
                {
                    Console.WriteLine("Error connecting to message broker...");
                }
                else {
                    //Inicializo da DLL - vai estar a correr ate app encerrar
                    dll.Initialize(sendData, delay);
                }
                }catch (Exception e)
                {
                    Console.WriteLine("erro a enviar mensagem");
                    Console.WriteLine(e);
                }
    Console.ReadKey();
        }
        private static void sendData(string str)
        {
            //crio um objecto SensorData com os valores correctos
            SensorData sensorData = new SensorData(str);

            //chamo metodo que devolve os dados do objecto em XML
            XmlDocument sensorXML = sensorData.getDataInXML();
            
            bool validationErrors = false;
            sensorXML.Schemas.Add(schema);
           
            sensorXML.Validate((s, e) =>
                {
                    //Apresento mensagem de erro por o XML nao ser valido
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(e.Message);
                    validationErrors = true;
                    Console.ResetColor(); 

                });
                              
                //publico o XML devidamente validado - Mosquitto
            if (!validationErrors)
            {
                 
                       // m_cClient.CleanSession();

                        string xmlOutput = sensorXML.OuterXml;
                        m_cClient.Publish("dataSensor", Encoding.UTF8.GetBytes(xmlOutput), 2,true);
                       
                     //   Console.WriteLine("Mensagem Publicada");
                    //   Console.WriteLine(xmlOutput);
                        

                    //m_cClient.Unsubscribe(m_strTopicsInfo); //Put this in a button to see notif!
                    //m_cClient.Disconnect(); //Free process and process's resources

            }         
        }

        private static void m_cClient_MsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("Mensagem Publicada");
        }
    }
}
