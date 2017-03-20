using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Chat.Hubs
{
    public class MainHub : Hub
    {
        public override async Task OnConnected()
        {
            await Clients.All.onMain<string>("Hello World!");
        }
    }
}
