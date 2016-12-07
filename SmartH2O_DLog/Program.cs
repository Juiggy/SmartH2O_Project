
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
using System.Net;

namespace SmartH2O_DLog
{
    class Program
    {
        //nao houve erros, publico mensagem
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

            Console.WriteLine("Mensagem recebida");


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