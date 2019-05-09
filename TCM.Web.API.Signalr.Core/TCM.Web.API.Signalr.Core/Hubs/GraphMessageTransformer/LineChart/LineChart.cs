
using DashboardMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class LineChart : QueryMessageTransformer
    {
        public override DashboardData ProcessRequest(DashboardData dashboardData)
        {
            DashboardData tranformedData = null;

            if (dashboardData.ViewType == DashboardViewType.LineChart)
            {
            }
            else
            {
                tranformedData = successor.ProcessRequest(dashboardData);
            }
            return tranformedData;
        }
    }
}