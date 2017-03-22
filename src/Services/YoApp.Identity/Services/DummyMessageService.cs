using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YoApp.Identity.Services.Interfaces;

namespace YoApp.Identity.Services
{
    public class DummyMessageService : ISmsSender
    {
        private ILogger _logger;

        public DummyMessageService(ILogger<DummyMessageService> logger)
        {
            _logger = logger;
        }

        public Task<bool> SendMessageAsync(string number, string message)
        {
            _logger.LogInformation($"DUMMY MESSAGE SENDER\nReceiver:{number}\nMessage:{message}");
            return Task.FromResult(true);
        }
    }
}
