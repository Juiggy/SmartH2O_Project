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
        static SensorNodeDll.SensorNodeDll dll;
        static bool auxFlag;
        static void Main(string[] args)
        {

            dll = new SensorNodeDll.SensorNodeDll(); 
            //MENU
            int option;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(20, 0); Console.WriteLine("Welcome To SmartH2O Data Uploader");
                Console.SetCursorPosition(8, 1); Console.WriteLine("Developers: Joana Vilhena | Joel Rodrigues | Sergio Batista");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Please Enter Your Choice: \n\n\n 1. Start Application \n 2. Debug \n 3. Settings \n 0. Exit \n ");
                Console.WriteLine("------------------");

                option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        {
                            
                            Console.Clear();
                            try
                            {
                                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLSensorDataMsgSchema.xsd";
                                schema.Add("", schemaPath);
                            }
                            catch (Exception e)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("erro a fazer load do schema");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }
                            try
                            {
                                m_cClient = new MqttClient(SmartH2O_Data_Uploader.Properties.Settings.Default.BrookerIP);
                            }
                            catch( Exception e)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("erro a fazer ligacao ao brooker - IP incorrecto");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }                           
                                                  
                            try
                            {
                                int delay = SmartH2O_Data_Uploader.Properties.Settings.Default.sensorTime;
                      //          m_cClient.MqttMsgPublished += m_cClient_MsgPublished;
                                if (!m_cClient.IsConnected)
                                {
                                    m_cClient.Connect(Guid.NewGuid().ToString());
                                    
                                }
                                bool opSair = false;
                                //criar listener que mete true com conjunto de teclas ou signal

                                Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
                                auxFlag = false;
                                dll.Initialize(sendData, delay);
                                
                                do
                                {

                                } while (!auxFlag);
                                /*
                                if (!m_cClient.IsConnected)
                                {
                                    Console.WriteLine("Error connecting to message broker...");
                                }
                                else
                                {
                                    //Inicializo da DLL - vai estar a correr ate app encerrar
                                    dll.Initialize(sendData, delay);
                                }*/
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("erro a enviar mensagem");
                                Console.WriteLine(e);
                            }
                            
                        }
                        break;
                    case 2:
                        {
                            

                        }
                        break;
                    case 3:
                        {
                            

                        }
                        break;

                    case 0:
                        {
                            Environment.Exit(0);

                        }
                        break;
                }

            }
            while (option != 0);          
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

                 string xmlOutput = sensorXML.OuterXml;
                 m_cClient.Publish("dataSensor", Encoding.UTF8.GetBytes(xmlOutput), 2,true);
              //   Console.WriteLine("Mensagem Publicada");

            }         
        }

        private static void m_cClient_MsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (!auxFlag)
                Console.WriteLine("Mensagem Publicada");

        }
        private static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            dll.Stop();
            args.Cancel = true;
            auxFlag = true;
        }
    }
}
