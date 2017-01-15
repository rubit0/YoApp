using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YoApp.Backend.Services.Interfaces;


namespace YoApp.Backend.Services
{
    public class TwilioMessageSender : IMessageSender
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioSender;
        private readonly ILogger _logger;

        public TwilioMessageSender(ILogger<TwilioMessageSender> logger)
        {
            _logger = logger;
            _accountSid = Startup.Configuration["Twillio:Sid"];
            _authToken = Startup.Configuration["Twillio:Token"];
            _twilioSender = Startup.Configuration["Twillio:Sender"];
        }

        public async Task<bool> SendMessageAsync(string number, string message)
        {
            _logger.LogInformation($"Attempting an (SMS) message delivery via Twilio to {number} from {_twilioSender}.");

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
            return new Uri($"https://api.twilio.com/2010-04-01/Accounts/{this._accountSid}/Messages.json");
        }

        private FormUrlEncodedContent GetTwilioFormHeaders(string receiver, string message)
        {
            var headers = new Dictionary<string, string>()
            {
                {"From", _twilioSender},
                {"To", receiver},
                {"Body", message}
            };

            return new FormUrlEncodedContent(headers);
        }

        private AuthenticationHeaderValue GetBasicAuthHeader()
        {
            var authValues = Encoding.ASCII.GetBytes($"{_accountSid}:{_authToken}");
            var converted = Convert.ToBase64String(authValues);

            return new AuthenticationHeaderValue("Basic", converted);
        }

        private class TwilioMessageSendResult
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            private readonly string[] _badStatusCodes = {"400", "undelivered", "failed", ""};

            public bool IsSuccess()
            {
                foreach (var badStatusCode in _badStatusCodes)
                {
                    if (string.CompareOrdinal(this.Status, badStatusCode) == 0)
                        return false;
                }

                return true;
            }
        }
    }
}
