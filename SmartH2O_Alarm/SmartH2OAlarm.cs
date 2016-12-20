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
        static XmlSchemaSet schemaAlarmMsg = new XmlSchemaSet();
        static XmlDocument triggerXML = new XmlDocument();
        static void Main(string[] args)
        {
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
                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\trigger-rules.xsd";


                //schema alarmes feito as trigger-rules.xmd
                schemaAlarm.Add("", schemaPath);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("error loading schema for alarm trigger rules");
                Console.ReadKey();
                Environment.Exit(1);
            }
            try
            {
                string schemaPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\XMLAlarmDataMsgSchema.xsd";
                schemaAlarmMsg.Add("", schemaPath);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("error loading schema for message alarm");
                Console.ReadKey();
                Environment.Exit(1);
            }
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

                            // tenho de verificar se o m_cClient já existe
                            if (aux_m_cClient)

                                try
                                {
                                    aux_m_cClient = false;
                                    m_cClient = new MqttClient(SmartH2O_Alarm.Properties.Settings.Default.brokerIP);
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
                                    string[] topicos = { "dataSensor" };

                                    byte[] qqos = { 2 };
                                    m_cClient.Subscribe(topicos, qqos);

                                }
                                m_cClient.MqttMsgPublished += m_cClient_MsgPublished;
                                m_cClient.MqttMsgPublishReceived += m_cClient_MqttMsgPublishReceived;
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
                            //quero mostrar todas as regras que existem, e se estao activas ou nao
                            Console.Clear();
                            Console.WriteLine("------------------- RULES -------------------");
                            createTriggerXMLwithSchema(triggerXML);
                            bool validationErrorsTrigger = false;
                            triggerXML.Validate((s, aux) =>
                            {
                                //Apresento mensagem de erro por o XML nao ser valido
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(aux.Message);
                                validationErrorsTrigger = true;
                                Console.ResetColor();
                            });
                            if (!validationErrorsTrigger)
                            {
                                XmlNode root = triggerXML.DocumentElement;
                                XmlNodeList nodeList = root.SelectNodes("//ALARM");
                                
                                foreach (XmlNode alarm in nodeList)
                                {

                                    //para sensor
                                    if (alarm["PARAMETER"] != null)
                                    {
                                        if (alarm["OPERATION"].InnerText.Equals("between"))
                                        {
                                            Console.WriteLine("status: " + ((alarm["CONDITION"].InnerText=="true") ? "ACTIVATED":"DEACTIVATED")+ " | generate alarm if sensor type is: "
                                                + alarm["PARAMETER"].InnerText + " and has value "
                                                + alarm["OPERATION"].InnerText
                                                + " " + alarm["VALUE"].InnerText
                                                + " and " + alarm["VALUE2"].InnerText + "\n----With Message----");
                                            Console.BackgroundColor = ConsoleColor.DarkRed;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine(alarm["MESSAGE"].InnerText + "\n\n");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.WriteLine("status: " + ((alarm["CONDITION"].InnerText == "true") ? "ACTIVATED" : "DEACTIVATED") + " | generate alarm if sensor type is: "
                                                + alarm["PARAMETER"].InnerText + " and has value "
                                                + alarm["OPERATION"].InnerText
                                                + " " + alarm["VALUE"].InnerText + "\n----With Message----");
                                            Console.BackgroundColor = ConsoleColor.DarkRed;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine(alarm["MESSAGE"].InnerText + "\n\n");
                                            Console.ResetColor();
                                        }
                                    }
                                    else
                                    {

                                        if (alarm["OPERATION"].InnerText.Equals("between"))
                                        {
                                            Console.WriteLine("status: " + ((alarm["CONDITION"].InnerText == "true") ? "ACTIVATED" : "DEACTIVATED") + " | generate alarm if sensor ID is: "
                                                + alarm["ID"].InnerText + " and has value "
                                                + alarm["OPERATION"].InnerText
                                                + " " + alarm["VALUE"].InnerText
                                                + " and " + alarm["VALUE2"].InnerText + "\n----With Message----");
                                            Console.BackgroundColor = ConsoleColor.DarkRed;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine(alarm["MESSAGE"].InnerText + "\n\n");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.WriteLine("status: " + ((alarm["CONDITION"].InnerText == "true") ? "ACTIVATED" : "DEACTIVATED") + " | generate alarm if sensor ID is: "
                                                + alarm["ID"].InnerText + " and has value "
                                                + alarm["OPERATION"].InnerText
                                                + " then " + alarm["VALUE"].InnerText + "\n----With Message----");
                                            Console.BackgroundColor = ConsoleColor.DarkRed;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine(alarm["MESSAGE"].InnerText + "\n\n");
                                            Console.ResetColor();
                                        }
                                    }

                                }
                                Console.ReadKey();
                            }
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
            if (e.Topic.Equals("dataSensor"))
            {
                XmlDocument sensorXML = new XmlDocument();
                sensorXML.LoadXml(Encoding.UTF8.GetString(e.Message));

                sensorXML.Schemas.Add(schemaSensor);


                bool validationErrors = false;
                sensorXML.Validate((s, aux) =>
                {
                    //Apresento mensagem de erro por o XML nao ser valido
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(aux.Message);
                    validationErrors = true;
                    Console.ResetColor();
                });


                if (!validationErrors)
                {
                    checkForAlarm(sensorXML);
                }

            }
        }

        private static void checkForAlarm(XmlDocument sensorXML)
        {

            XmlDocument alarmXML = new XmlDocument();
            createTriggerXMLwithSchema(triggerXML);

            bool validationErrorsTrigger = false;
            triggerXML.Validate((s, aux) =>
            {
                //Apresento mensagem de erro por o XML nao ser valido
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(aux.Message);
                validationErrorsTrigger = true;
                Console.ResetColor();
            });
            if (!validationErrorsTrigger)
            {
                XmlNode root = triggerXML.DocumentElement;

                XmlNodeList nodeList = root.SelectNodes("//ALARM[ID=" + sensorXML.SelectSingleNode("/parameter/idSensor[1]").InnerText + "]");
                checkNodeList(sensorXML, nodeList);
                XmlNodeList nodeListParameter = root.SelectNodes("//ALARM[PARAMETER='" + sensorXML.SelectSingleNode("/parameter/type[1]").InnerText + "']");
                checkNodeList(sensorXML, nodeListParameter);
            }
        }

        private static void createTriggerXMLwithSchema(XmlDocument triggerXML)
        {
            try
            {
                triggerXML.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_data\\trigger-rules.xml");

            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("error loading trigger-rules.xml");
                Console.WriteLine(e.Message);
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(1);
            }
            triggerXML.Schemas.Add(schemaAlarm);
        }

        private static void checkNodeList(XmlDocument sensorXML, XmlNodeList nodeList)
        {
            foreach (XmlNode alarm in nodeList)
            {
                //s
                if (bool.Parse(alarm["CONDITION"].InnerText) == true)
                {
                    //switch que verifica a condicao. Mediante a condicao, verifica se gera alarme ou nao
                    //decidir se ao fim de um alarme continua a correr as outras regras ou não

                    string valorsensorStr = sensorXML.SelectSingleNode("/parameter/value").InnerText;
                    valorsensorStr = valorsensorStr.Replace(".", ",");
                    float valorSensor = float.Parse(valorsensorStr);
                    string valorAlarmStr = alarm["VALUE"].InnerText;
                    valorAlarmStr = valorAlarmStr.Replace(".", ",");
                    float valorAlarm = float.Parse(valorAlarmStr);


                    switch (alarm["OPERATION"].InnerText)
                    {
                        //verifico a condição
                        case "equal":
                            if (valorSensor == valorAlarm)
                            {
                                //tem erro! enviar mensagem
                                sendMsgAlarm(alarm, sensorXML);
                            }
                            break;
                        case "less than":
                            if (valorSensor < valorAlarm)
                            {
                                //tem erro! enviar mensagem
                                sendMsgAlarm(alarm, sensorXML);
                            }
                            break;
                        case "greater than":
                            if (valorSensor > valorAlarm)
                            {
                                //tem erro! enviar mensagem
                                sendMsgAlarm(alarm, sensorXML);
                            }
                            break;
                        case "between":

                            string valor2AlarmStr = alarm["VALUE2"].InnerText;
                            valor2AlarmStr = valor2AlarmStr.Replace(".", ",");
                            float valor2Alarm = float.Parse(valor2AlarmStr);

                            if ((valorAlarm < valorSensor) && (valorSensor < valor2Alarm))
                            {
                                //tem erro! enviar mensagem
                                sendMsgAlarm(alarm, sensorXML);
                            }
                            break;
                    }


                }
            }
        }

        private static void sendMsgAlarm(XmlNode alarm, XmlDocument sensorXML)
        {

            Console.WriteLine("Sensor: "+sensorXML.SelectSingleNode("/parameter/idSensor[1]").InnerText+" has given an Alarm");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(alarm["MESSAGE"].InnerText);
            Console.ResetColor();

            //criar mensagem para publicar via mosquitto
            //Vou criar primeiro xml document com a mensagem
            //depois vou validar com schema
            //se for valido publico para o mosquitto

            //criar ficheiro xml com a mensagem
            XmlDocument docAlarm = new XmlDocument();
            XmlNode sensorNode = sensorXML.SelectSingleNode("parameter");
            XmlElement alarmMsg = docAlarm.CreateElement("alarm");
            
            alarmMsg.InnerXml = sensorNode.InnerXml;
            docAlarm.AppendChild(alarmMsg);
            XmlElement message = docAlarm.CreateElement("message");
            message.InnerText = alarm["MESSAGE"].InnerText;
            alarmMsg.AppendChild(message);

            //validar com schema
            docAlarm.Schemas.Add(schemaAlarmMsg);
            bool validationErrorsAlarm = false;
            docAlarm.Validate((s, aux) =>
            {
                //Apresento mensagem de erro por o XML nao ser valido
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(aux.Message);
                validationErrorsAlarm = true;
                Console.ResetColor();
            });
            if (!validationErrorsAlarm)
            {
                //validei com o schema. Posso publicar mensagem no mosquitto
                string xmlOutput = docAlarm.OuterXml;
                m_cClient.Publish("dataAlarm", Encoding.UTF8.GetBytes(xmlOutput), 2, false);
            }
        }

        private static void m_cClient_MsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            //throw new NotImplementedException();
            Console.WriteLine(DateTime.Now + " - Alarm Send | Program is running... | Press ESC to quit\n");
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
