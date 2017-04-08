using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.SignalR;
using YoApp.Chat.Hubs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YoApp.Chat.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private ILogger _logger;
        private IHubContext _hubContext;

        public MessagesController(ILogger<MessagesController> logger)
        {
            _logger = logger;
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(string receiver, string message)
        {
            if (string.IsNullOrWhiteSpace(receiver))
                return BadRequest("No receiver.");

            _logger.LogInformation($"Sending chat message from{User.Identity.Name} to {receiver}");

            await _hubContext.Clients.User(receiver).OnReceiveMessage(User.Identity.Name, message);
            return Ok();
        }
    }
}
