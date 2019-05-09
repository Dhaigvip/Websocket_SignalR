//using FlightControlCore.Message;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Threading.Tasks;

//namespace DashboardMessage
//{
//    [DataContract(Namespace = "http://www.tieto.com/xfm/dashboard")]

//    public class DashBoardRefresh : IMessage
//    {
//        public DashBoardRefresh()
//        {
//            QueryId = new List<string>();
//        }

//        [DataMember(EmitDefaultValue = false)]
//        public List<string> QueryId { get; private set; }
//    }
//    public enum DashboardViewType
//    {
//        TextBox,
//        List,
//        Table,
//        PieChart,
//        BarChart,
//        LineChart,
//        SimplePie
//    }

//    [DataContract(Namespace = "http://www.tieto.com/xfm/dashboard")]
//    public class DashboardData : IMessage
//    {
//        [DataMember(EmitDefaultValue = false)]
//        public string QueryId { get; set; }

//        [DataMember(EmitDefaultValue = false)]
//        public string Data { get; set; }

//        [DataMember(EmitDefaultValue = false)]
//        public DashboardViewType ViewType { get; set; }

//        [DataMember(EmitDefaultValue = false)]
//        public string ViewRole { get; set; }

//        [DataMember(EmitDefaultValue = false)]
//        public string[][] MetaDictionary { get; set; }

//        [DataMember(EmitDefaultValue = false)]
//        public string Description { get; set; }
//    }
//}
