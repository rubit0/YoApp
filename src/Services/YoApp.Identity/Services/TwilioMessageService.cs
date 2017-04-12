using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YoApp.Identity.Core;
using YoApp.Identity.Services.Interfaces;

namespace YoApp.Identity.Services
{
    public class TwilioMessageService : ISmsSender
    {
        private readonly ILogger _logger;
        private readonly IConfigurationService _configurationService;

        public TwilioMessageService(ILogger<TwilioMessageService> logger, IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        public async Task<bool> SendMessageAsync(string number, string message)
        {
            _logger.LogInformation($"Attempting (SMS) message delivery via Twilio to {number} from {_configurationService.Twillio.SenderPhoneNumber}.");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = this.GetBasicAuthHeader();
            var response = await client.PostAsync(this.GetTwilioEndpoint(), this.GetTwilioFormHeaders(number, message));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Twilio denied service, message to [{number}] could not be send.\nHttp Status: [{response.StatusCode}]\nReason: [{response.ReasonPhrase}]");
                return false;
            }
            
            var contentStream = await response.Content.ReadAsStringAsync();
            var status = JsonConvert.DeserializeObject<TwilioMessageSendResult>(contentStream);

            if (!status.IsSuccess())
            {
                _logger.LogWarning($"Could not send message to [{number}].\nTwilio Error Status: [{status.Status}]");
                return false;
            }

            _logger.LogInformation($"SMS message was delivered to [{number}] by Twilio successfully.");
            return true;
        }

        private Uri GetTwilioEndpoint()
        {
            return new Uri($"https://api.twilio.com/2010-04-01/Accounts/{_configurationService.Twillio.AccountSid}/Messages.json");
        }

        private FormUrlEncodedContent GetTwilioFormHeaders(string receiver, string message)
        {
            var headers = new Dictionary<string, string>()
            {
                {"From", _configurationService.Twillio.SenderPhoneNumber},
                {"To", receiver},
                {"Body", message}
            };

            return new FormUrlEncodedContent(headers);
        }

        private AuthenticationHeaderValue GetBasicAuthHeader()
        {
            var authValues = Encoding.ASCII.GetBytes($"{_configurationService.Twillio.AccountSid}:{_configurationService.Twillio.AuthToken}");
            var converted = Convert.ToBase64String(authValues);

            return new AuthenticationHeaderValue("Basic", converted);
        }

        private class TwilioMessageSendResult
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            private static readonly string[] BadStatusCodes = {"400", "undelivered", "failed", ""};

            public bool IsSuccess()
            {
                return !BadStatusCodes.Contains(Status);
            }
        }
    }
}
