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

                Console.WriteLine("tudo okay");
                string xmlOutput = sensorXML.OuterXml;
                Console.WriteLine(xmlOutput);
            }
            
         
        }

        
    }
}
