using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt; //lib para Mosquitto
using uPLibrary.Networking.M2Mqtt.Messages; //lib para Mosquitto
using System.Xml;
using System.Xml.Schema;

namespace SmartH2O_Alarm
{
    class SmartH2OAlarm
    {
        static MqttClient m_cClient;
        static XmlSchemaSet schemaSensor = new XmlSchemaSet();
        static XmlSchemaSet schemaAlarm = new XmlSchemaSet();

        static void Main(string[] args)
        {
        
            //MENU
            int option;
            bool aux_m_cClient = true;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(20, 0); Console.WriteLine("Welcome To SmartH2O_Alarm");
                Console.SetCursorPosition(8, 1); Console.WriteLine("Developers: Joana Vilhena | Joel Rodrigues | Sergio Batista");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Please Enter Your Choice: \n\n 1. Start Application \n 2. View rules \n 3. Set IP for Broker \n 0. Exit \n ");
                Console.WriteLine("------------------");
                option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        {
                            //Start Application
                            Console.Clear();
                            try
                            {
                                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLSensorDataMsgSchema.xsd";
                                schemaSensor.Add("", schemaPath);
                            }
                            catch (Exception e)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("error loading schema for sensor");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }
                            try
                            {
                                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLSensorDataMsgSchema.xsd";


                                //falta fazer schema para os alarmes

                       

                                schemaAlarm.Add("", schemaPath);
                            }
                            catch (Exception e)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("error loading schema for alarm");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }

                            // tenho de verificar se o m_cClient já existe
                            if (aux_m_cClient)
                                try
                                {
                                    aux_m_cClient = false;
                                    m_cClient = new MqttClient(SmartH2O_Alarm.Properties.Settings.Default.brokerIP);
                                    m_cClient.MqttMsgPublished += m_cClient_MsgPublished;
                                    m_cClient.MqttMsgPublishReceived += m_cClient_MqttMsgPublishReceived;

                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("error connecting with broker");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                }
                                if (!m_cClient.IsConnected)
                                {
                                    m_cClient.Connect(Guid.NewGuid().ToString());

                                }
                                Console.WriteLine("Press ESC to quit\n---------------------------------");
                                do
                                {
                                    //fica a escrever na janela até clicar na tecla ESC
                                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                            m_cClient.MqttMsgPublishReceived -= m_cClient_MqttMsgPublishReceived;
                            m_cClient.MqttMsgPublished -= m_cClient_MsgPublished;
                            Console.Clear();

                        }
                        break;
                    case 2:
                        {
                            //check alarms
                        }
                        break;
                    case 3:
                        {
                            Console.Clear();
                            Console.WriteLine("Brokers IP: " + SmartH2O_Alarm.Properties.Settings.Default.brokerIP);
                            Console.Write("\nSet the new IP for the Broker: ");
                            string broker = Console.ReadLine();

                            if (ValidateIPv4(broker))
                            {
                                if (!aux_m_cClient)
                                {
                                    aux_m_cClient = true;
                                    if (m_cClient.IsConnected)
                                    {
                                        m_cClient.Disconnect();
                                    }

                                }
                                SmartH2O_Alarm.Properties.Settings.Default.brokerIP = broker;
                                SmartH2O_Alarm.Properties.Settings.Default.Save();
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

        private static void m_cClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void m_cClient_MsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //metodo retirado do stackoverflow
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
