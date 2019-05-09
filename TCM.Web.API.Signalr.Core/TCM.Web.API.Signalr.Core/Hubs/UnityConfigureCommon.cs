using CommonCore.Tcm.Common.UnityContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace TCM.Web.API.Signalr.Core.Hubs
{
    public class UnityConfigureCommon : IUnityConfigure
    {
        public IUnityContainer Init(IUnityContainer cnt)
        {
            //cnt.RegisterType<CommonCore.Tcm.Common.Tcm.Logger.ILogger, TeLog4Net>(new ContainerControlledLifetimeManager());


            return cnt;
        }

        public string Environment
        {
            get { return "Common"; }
        }
    }
}
