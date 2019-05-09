using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using CommonCore.Tcm.Common.UnityContainer;
using TCM.Web.API.Signalr.Core;

namespace TCM.Web.API.Core.Signalr
{
    //[HubName("DashboardQueryHub")]
    public class DashboardQueryHub : Hub
    {
        private IDashboarQueryHandler _dashboarQueryHandler;

        public DashboardQueryHub()
        {
            _dashboarQueryHandler = (DashboarQueryConsumer)UnityHelper.Container.Resolve(typeof(DashboarQueryConsumer), "XfmClientConsumer");

            if (_dashboarQueryHandler == null)
            {
                throw new ArgumentNullException("IDashboarQueryHandler");
            }
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task<bool> RegisterUser(JObject c)
        {
            //TO DO:
            //return true;
            //Need support for user groups...
            Context cc = JsonConvert.DeserializeObject<Context>(c.ToString());
            await _dashboarQueryHandler.RegisterUser(cc.RegUser, cc.RegUser.ToString(), Context.ConnectionId);
            return true;

        }
    }
}