using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCM.Web.API.Signalr.Core
{
    public class MyMqSettings
    {
        public List<MqConnection> Connections { get; set; }
       
    }   
    public class MqConnection
    {
        public string name { get; set; }
        public string QueueManager { get; set; }
        public string ReadQueueName { get; set; }
        public string WriteQueueName { get; set; }
        public string CodePage { get; set; }
        public string DynamicQueueName { get; set; }
        public List<KeyValue> Properties { get; set; }
    }
    public class KeyValue
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
