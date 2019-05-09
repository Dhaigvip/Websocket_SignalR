using DashboardMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class PieChart : QueryMessageTransformer
    {
        public override DashboardData ProcessRequest(DashboardData dashboardData)
        {

            dynamic dashboardDataDynamic = new System.Dynamic.ExpandoObject();
            dashboardDataDynamic.B = new System.Dynamic.ExpandoObject();


            DashboardData tranformedData = null;

            if (dashboardData.ViewType == DashboardViewType.PieChart)
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