using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SoftwareOrganizationSmartH2O
{
    class AlarmData
    {
        private string _parameter;
        private string parameter
        {
            get;set;
        }

        private decimal _value;
        private decimal value
        {
            get;set;
        }

        private decimal _value2;
        private decimal value2
        {
            get;set;
        }

        private string _operation;
        private string operation
        {
            get;set;
        }

        private bool _condition;
        private bool condition
        {
            get;set;
        }

        private string _message;
        private string message
        {
            get; set;
        }


        //contruir com o XML

        public AlarmData(XmlDocument alarmxml)
        {
            this._parameter = alarmxml.SelectSingleNode("ALARM/PARAMETER").InnerText;
            this._condition = bool.Parse(alarmxml.SelectSingleNode("ALARM/CONDITION").InnerText);
            this._value = decimal.Parse(alarmxml.SelectSingleNode("ALARM/VALUE").InnerText);
            this._value2 = decimal.Parse(alarmxml.SelectSingleNode("ALARM/VALUE2").InnerText);
            this._operation = alarmxml.SelectSingleNode("ALARM/OPERATION").InnerText;
            this._message = alarmxml.SelectSingleNode("ALARM/MESSAGE").InnerText;

        }

        //

        public XmlDocument getDatainXML()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlElement alarm = doc.CreateElement("ALARM");
            doc.AppendChild(alarm);



            XmlElement parameter = doc.CreateElement("PARAMETER");
            parameter.InnerText = _parameter;

            // ate aqui ok!
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
