using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class ChartColorOptions
    {

        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public string pointBorderColor { get; set; }
        public string pointHoverBackgroundColor { get; set; }
        public string pointHoverBorderColor { get; set; }
    }
}