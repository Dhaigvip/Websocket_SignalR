using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class ChartOptions
    {
        public ChartOptions()
        {
            this.responsive = true;
        }
        public bool responsive { get; set; }
    }
}