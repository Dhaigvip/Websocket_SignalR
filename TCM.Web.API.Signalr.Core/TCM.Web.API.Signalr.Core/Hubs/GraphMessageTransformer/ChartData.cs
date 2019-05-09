using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class ChartData
    {
        public ChartData()
        {
            data = new List<decimal>();
        }
        public List<decimal> data { get; set; }
        public string label { get; set; }
    }
}