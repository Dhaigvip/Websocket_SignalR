using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class ChartObject
    {
        public ChartObject()
        {
            BarChartData = new List<ChartData>();
            Labels = new List<string>();
            //Colors = new List<BarChartColorOptions>();
        }

        public List<ChartData> BarChartData { get; set; }
        public List<string> Labels { get; set; }
        public ChartOptions Options { get; set; }

        //public List<BarChartColorOptions> Colors { get; set; }
        public bool Legend { get; set; }

        public string ChartType { get; set; }
    }
}