
using DashboardMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public abstract class QueryMessageTransformer
    {
        protected QueryMessageTransformer successor;

        public void SetSuccessor(QueryMessageTransformer successor)
        {
            this.successor = successor;
        }

        public abstract DashboardData ProcessRequest(DashboardData dashboardData);
    }
}