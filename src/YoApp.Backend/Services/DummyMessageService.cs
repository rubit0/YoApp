using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Backend.Services.Interfaces;

namespace YoApp.Backend.Services
{
    public class DummyMessageService : ISmsSender
    {
        private ILogger _logger;

        public DummyMessageService(ILogger<DummyMessageService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync(string number, string message)
        {
            _logger.LogInformation($"DUMMY MESSAGE SENDER\nReceiver:{number}\nMessage:{message}");
            return true;
        }
    }
}
