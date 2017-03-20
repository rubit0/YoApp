using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Chat.Hubs
{
    [Authorize]
    public class MainHub : Hub
    {
        public MainHub()
        {
        }

        public override async Task OnConnected()
        {
            await Clients.All.OnWelcome<string>("Hello World!");
        }
    }
}
