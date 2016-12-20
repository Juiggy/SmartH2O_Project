using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
        [OperationContract]
        string WriteDataSensor(string strMsg);
        [OperationContract]
        string WriteDataAlarm(string strMsg);
        [OperationContract]
        string getDataAlarmXML();
        [OperationContract]
        string getDataSensorXML();
        [OperationContract]
        List<string> getParameterMinRangeDay(string parameter, string dateBg, string dateEd);
        [OperationContract]
        List<string> getParameterMaxRangeDay(string parameter, string dateBg, string dateEd);
        [OperationContract]
        List<string> getParameterAvgRangeDay(string parameter, string dateBg, string dateEd);
        [OperationContract]
        string[] getParameterMinHourInDay(string parameter, string day);
        [OperationContract]
        string[] getParameterMaxHourInDay(string parameter, string day);
        [OperationContract]
        string[] getParameterAvgHourInDay(string parameter, string day);
        [OperationContract]
        List<string> getParameterMinWeekly(string parameter, int week);
        [OperationContract]
        List<string> getParameterMaxWeekly(string parameter, int week);
        [OperationContract]
        List<string> getParameterAvgWeekly(string parameter, int week);

        [OperationContract]
        List<string> getAlarmRangeDay(List<string> parameters, string dateBg, string dateEd);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    
}
