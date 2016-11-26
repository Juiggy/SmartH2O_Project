using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SoftwareOrganizationSmartH2O
{
    public class SensorData
    {



        private string _type;
        public string Type
        {
            get { return _type; }
            //set { _type = value; }
        }

        private float _value;
        public float Value
        {
            get { return _value; }
            //set { _value = value; }
        }


        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            // set { _date = value; }
        }

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            //set { _id = value; }
        }

        public SensorData(string tipo, string valor)
        {
            this._type = tipo;
            valor = valor.Replace(".", ",");
            this._value = float.Parse(valor);
            this._id = Guid.NewGuid();
            this._date = DateTime.Now;
        }

        public SensorData(string sensorValue)
        {
            String[] sensorValues = sensorValue.Split(';');
            this._type = sensorValues[1];
            string valor = sensorValues[2];
            valor = valor.Replace(".", ",");
            this._value = float.Parse(valor);
            this._id = Guid.NewGuid();
            this._date = DateTime.Now;
        }

        public XmlDocument getDataInXML()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlElement sensor = doc.CreateElement("sensor");
            doc.AppendChild(sensor);
           
            

            XmlElement tipo = doc.CreateElement("type");
            tipo.InnerText = _type;
            XmlElement id = doc.CreateElement("id");
            id.InnerText = _id.ToString();
            XmlElement date = doc.CreateElement("date");
            date.InnerText = _date.ToString(); //terei de verificar o formato da hora?
            XmlElement value = doc.CreateElement("value");
            string aux = _value.ToString();
            aux = aux.Replace(",", ".");
            value.InnerText = aux;

            sensor.AppendChild(tipo);
            sensor.AppendChild(id);
            sensor.AppendChild(date);
            sensor.AppendChild(value);
         


            return doc;
        }
    }

}
