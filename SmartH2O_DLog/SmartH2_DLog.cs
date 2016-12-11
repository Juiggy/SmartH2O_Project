﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using uPLibrary.Networking.M2Mqtt; //lib para Mosquitto
using uPLibrary.Networking.M2Mqtt.Messages; //lib para Mosquitto
using System.Xml;
using System.Xml.Schema;
using System.IO;
using SoftwareOrganizationSmartH2O;
using System.Net;
using SmartH2O_DLog;

namespace SmartH2O_DLog
{
    class SmartH2_DLog
    {
        static XmlSchemaSet schemaSensor = new XmlSchemaSet();
        static XmlSchemaSet schemaAlarm = new XmlSchemaSet();
        static MqttClient m_cClient;
        static void Main(string[] args)
        {
            bool aux_m_cClient = true;
            //MENU
            int option;
            
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(20, 0); Console.WriteLine("Welcome To SmartH2O_DLog");
                Console.SetCursorPosition(8, 1); Console.WriteLine("Developers: Joana Vilhena | Joel Rodrigues | Sergio Batista");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Please Enter Your Choice: \n\n 1. Start Application \n 2. Download File \n 3. Set IP for Broker \n 0. Exit \n ");
                Console.WriteLine("------------------");
                option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        {
                            //para fazer apenas uma vez
                            try
                            {
                                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLSensorDataMsgSchema.xsd";
                                schemaSensor.Add("", schemaPath);
                            }
                            catch (Exception e)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("error loading schema for Data Sensor");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }
                            if (aux_m_cClient)
                            {
                                try
                                {
                                        aux_m_cClient = false;
                                        m_cClient = new MqttClient(SmartH2O_DLog.Properties.Settings.Default.brokerIP);


                                } catch(Exception e)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Cannot connect to broker");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                }
                            }

                            m_cClient.MqttMsgPublishReceived += m_cClient_MqttMsgPublishReceived;
                            Console.Clear();
                            if (!m_cClient.IsConnected)
                            {
                                try
                                {
                                    m_cClient.Connect(Guid.NewGuid().ToString());
                                    string[] topicos = { "dataSensor", "dataAlarme"};
                                 
                                    byte[] qqos = { 2,2 };
                                    m_cClient.Subscribe(topicos, qqos);
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine("Error connecting to broker");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                }
                            }
                            
                            Console.WriteLine("Press ESC to quit\n------------------------");
                            do
                            {
                                //para a janela ate clicar ESC
                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                            m_cClient.MqttMsgPublishReceived -= m_cClient_MqttMsgPublishReceived;
                        }
                        break;
                    case 2:
                        {
                          
                        }
                        break;
                    case 3:
                        {
                            Console.Clear();
                            Console.WriteLine("Brokers IP: " + SmartH2O_DLog.Properties.Settings.Default.brokerIP);
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
                                SmartH2O_DLog.Properties.Settings.Default.brokerIP = broker;
                                SmartH2O_DLog.Properties.Settings.Default.Save();
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

            Console.WriteLine(DateTime.Now + " - Message Received from: "+e.Topic.ToString()+" | Press ESC to quit");
            //criar ficheiro XML a partir da mensagem    
            XmlDocument documentoXML = new XmlDocument();
            documentoXML.LoadXml(Encoding.UTF8.GetString(e.Message));

            if (e.Topic.Equals("dataSensor")){




                //validar com schema
                bool validationErrors = false;
                documentoXML.Schemas.Add(schemaSensor);

                documentoXML.Validate((s, aux) =>
                {
                    //Apresento mensagem de erro por o XML nao ser valido
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(aux.Message);
                    validationErrors = true;
                    Console.ResetColor();
                });

                //validou? chamar método do webservice
                if (!validationErrors)
                {
                    //cria um objecto com o xml
                    SensorData aux = new SensorData(documentoXML);

                    //chamo o metodo do webservice para guardar estes valores
                    
                }

            }
            else
            {
                if (e.Topic.Equals("dataAlarme"))
                {

                    //validar com schema
                    //validou? chamar método do webservice


                }
            }
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