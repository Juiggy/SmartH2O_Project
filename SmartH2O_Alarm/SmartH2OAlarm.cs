using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartH2O_Alarm
{
    class SmartH2OAlarm
    {
        static void Main(string[] args)
        {
            //MENU
            int option;

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
