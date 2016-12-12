using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartH2O_Alarm
{
    [XmlRoot("ALARMSDETAILS")]
   public class ListAlarm
    {
        public ListAlarm()
        {
            Alarms = new List<Alarm>();
    
        }

        [XmlElement("ALARM")]
        public List<Alarm> Alarms { get; set; }

    }

    public class Alarm
    {
        [XmlElement ("PARAMETER")]
        public String Parameter { get; set; }

        [XmlElement("CONDITION")]
        public bool Condition { get; set; }

        [XmlElement("MINVALUE")]
        public decimal MinValue { get; set; }

        [XmlElement("MAXVLUE")]
        public decimal MaxValue { get; set; }
    }

    static class InitializeList
    {
        static void Main()
        {
            XmlSerializer s = new XmlSerializer(typeof(Alarm));
            ListAlarm list = new ListAlarm();

            


            s.Serialize(Console.Out, list);
        }
       
    }
}
