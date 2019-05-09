using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCM.Web.API.Signalr.Core
{
    public class DashboardSettings
    {
        public string DashboardInternalQueue { get; set; }
        public string WebspearReadQueue { get; set; }
        public string DashboardgatewayQueue { get; set; }
        public string DeadletterQueue { get; set; }

        public bool UseWebSpear { get; set; }

    }
}
