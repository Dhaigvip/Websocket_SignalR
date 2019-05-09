using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace TCM.Web.API.Signalr.Core
{
    public class TestHub : Hub
    {
        public async Task NewMessage(string username, string message)
        {
            await Clients.All.SendAsync("messageReceived", username, message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}