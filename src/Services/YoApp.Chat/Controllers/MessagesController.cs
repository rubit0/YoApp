﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.SignalR;
using YoApp.Chat.Hubs;
using System.Threading.Tasks;

namespace YoApp.Chat.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private IHubContext _hubContext;

        public MessagesController()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
        }

        public async Task<IActionResult> SendMessageAll(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest();

            await _hubContext.Clients.All.OnMessage(message);
            return Ok();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(string receiver, string message)
        {
            if (string.IsNullOrWhiteSpace(receiver))
                return BadRequest("No receiver.");

            await _hubContext.Clients.User(receiver).OnMessage(User.Identity.Name, message);
            return Ok();
        }
    }
}
