//  ******************************************************************
//  *  TCM.Web.Application
//  *  TCM.Web.Application - BarChart.cs
//  ******************************************************************



using DashboardMessage;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{




    public class BarChart : QueryMessageTransformer
    {
        public override DashboardData ProcessRequest(DashboardData dashboardData)
        {

            if (dashboardData.ViewType == DashboardViewType.BarChart || dashboardData.ViewType == DashboardViewType.LineChart)
            {
                var processor = new BarChartsProcessor();
                return processor.Process(dashboardData);
            }
            else
            {
                return successor.ProcessRequest(dashboardData);
            }
        }
    }
}