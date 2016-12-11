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
    class SmartH2O_DU
    {
        //nao houve erros, publico mensagem
        static MqttClient m_cClient;
        static XmlSchemaSet schema = new XmlSchemaSet();
        string[] m_strTopicsInfo = { "dataSensor" };
        static SensorNodeDll.SensorNodeDll dll;
        static bool auxFlag;
        static void Main(string[] args)
        {
            bool aux_m_cClient = true;
            //MENU
            int option;

            int delay = SmartH2O_Data_Uploader.Properties.Settings.Default.sensorTime;
            

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(20, 0); Console.WriteLine("Welcome To SmartH2O Data Uploader");
                Console.SetCursorPosition(8, 1); Console.WriteLine("Developers: Joana Vilhena | Joel Rodrigues | Sergio Batista");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Please Enter Your Choice: \n\n 1. Start Application \n 2. Set Delay Time \n 3. Set IP for Broker \n 0. Exit \n ");
                Console.WriteLine("------------------");
                option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        {
                            //escolho criar a dll aqui, porque depois de um stop ela nao volta a correr o metodo Initialize
                            dll = new SensorNodeDll.SensorNodeDll();
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
                                Console.WriteLine("error loading schema");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }
                            // tenho de verificar se o m_cClient já existe
                            if (aux_m_cClient)
                                try
                                {
                                    aux_m_cClient = false;
                                    m_cClient = new MqttClient(SmartH2O_Data_Uploader.Properties.Settings.Default.brokerIP);
                                    m_cClient.MqttMsgPublished += m_cClient_MsgPublished;
                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("error connecting with broker");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                }
                            try
                            {

                                if (!m_cClient.IsConnected)
                                {
                                    m_cClient.Connect(Guid.NewGuid().ToString());

                                }
                                Console.WriteLine("Press ESC to quit\n---------------------------------");
                                auxFlag = false;
                                dll.Initialize(sendData, delay);
                                do
                                {
                                    //fica a escrever na janela até clicar na tecla ESC
                                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                dll.Stop();
                                auxFlag = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("cannot send message");
                                Console.WriteLine(e);
                                Console.ReadKey();
                                Environment.Exit(1);
                            }
                            Console.Clear();
                        }
                        break;
                    case 2:
                        {
                            Console.Clear();
                            //ponderar passar a mostrar valor em segundo e ler em segundos - tlv minutos! falar com colegas
                            Console.WriteLine("Delay Time value (ms): " + delay.ToString());
                            Console.Write("\nSet new Delay time (ms): ");
                            //verificar se o que foi escrito é numerico
                            int value;
                            string delayString = Console.ReadLine();
                            if (int.TryParse(delayString, out value))
                                delay = Int32.Parse(delayString);
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("\n\n" + delayString + " is not a valid value");
                                Console.ResetColor();
                                Console.ReadKey();
                            }
                            //guardar valor delay do ecra
                            SmartH2O_Data_Uploader.Properties.Settings.Default.sensorTime = delay;
                            SmartH2O_Data_Uploader.Properties.Settings.Default.Save();
                        }
                        break;
                    case 3:
                        {
                            Console.Clear();
                            Console.WriteLine("Brokers IP: " + SmartH2O_Data_Uploader.Properties.Settings.Default.brokerIP);
                            Console.Write("\nSet the new IP for the Broker: ");
                            string broker = Console.ReadLine();

                            if (ValidateIPv4(broker))
                            {
                                SmartH2O_Data_Uploader.Properties.Settings.Default.brokerIP = broker;
                                SmartH2O_Data_Uploader.Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("\n\n" + broker + " is not a valid IP");
                                Console.ResetColor();
                                Console.ReadKey();
                            }
                        }
                        break;
                    case 0:
                        {
                            Console.Clear();
                            Console.WriteLine("\nThank you for using our program");
                            Console.ReadKey();
                            //fazer uma mensagem para sair
                        }
                        break;
                }
            }
            while (option != 0);
            Environment.Exit(0);
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
                m_cClient.Publish("dataSensor", Encoding.UTF8.GetBytes(xmlOutput), 2, true);
            }
        }

        private static void m_cClient_MsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (!auxFlag)
                Console.WriteLine(DateTime.Now + " - Message Send | Program is running... | Press ESC to quit");
        }

        //metodo retirado do Stackoverflow
        private static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

    }
}
