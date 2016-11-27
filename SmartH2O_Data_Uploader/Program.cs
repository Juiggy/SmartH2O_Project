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
        static void Main(string[] args)
        {

            SensorNodeDll.SensorNodeDll dll = new SensorNodeDll.SensorNodeDll();
            int delay = SmartH2O_Data_Uploader.Properties.Settings.Default.sensorTime;
                   
            //Inicializo da DLL - vai estar a correr ate app encerrar
            dll.Initialize(sendData, delay);

            Console.ReadKey();
        }
        private static void sendData(string str)
        {
            //crio um objecto SensorData com os valores correctos
            SensorData sensorData = new SensorData(str);

            //chamo metodo que devolve os dados do objecto em XML
            XmlDocument sensorXML = sensorData.getDataInXML();

          
            //valido o XML com o schema
            XmlSchemaSet schema = new XmlSchemaSet();
            
            //como fazer isto de forma mais dinamica?
            string schemaPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\SoftwareOrganizationSmartH2O\\XMLSensorDataMsgSchema.xsd";
            schema.Add("", schemaPath);
                       
                

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
                //nao houve erros, publico mensagem
                MqttClient m_cClient = new MqttClient(IPAddress.Parse(SmartH2O_Data_Uploader.Properties.Settings.Default.BrookerIP));
                string[] m_strTopicsInfo = { "dataSensor" };

                try
                { 
                   if(!m_cClient.IsConnected)
                        m_cClient.Connect(Guid.NewGuid().ToString());
                    
                    if (!m_cClient.IsConnected)
                    {
                        Console.WriteLine("Error connecting to message broker...");
                    
                    }else { 


                        string xmlOutput = sensorXML.OuterXml;
                        m_cClient.Publish("dataSensor", Encoding.UTF8.GetBytes(xmlOutput));

                        Console.WriteLine("Mensagem Publicada");
                    //   Console.WriteLine(xmlOutput);
                        

                    //m_cClient.Unsubscribe(m_strTopicsInfo); //Put this in a button to see notif!
                    //m_cClient.Disconnect(); //Free process and process's resources

                    }
                }catch (Exception e)
                {
                    Console.WriteLine("erro a enviar mensagem");
                    Console.WriteLine(e);
                }


            }


        }


    }
}
