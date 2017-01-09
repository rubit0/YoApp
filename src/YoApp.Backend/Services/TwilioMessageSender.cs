using System;
using System.Net;
using System.Threading.Tasks;
using YoApp.Backend.Services.Interfaces;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace YoApp.Backend.Services
{
    public class TwilioMessageSender : IMessageSender
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioSender;

        public TwilioMessageSender()
        {
            _accountSid = Startup.Configuration["Twillio:Sid"];
            _authToken = Startup.Configuration["Twillio:Token"];
            _twilioSender = Startup.Configuration["Twillio:Sender"];
        }

        public async Task SendMessageAsync(string number, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            try
            {
                var messageTask = await MessageResource.CreateAsync(
                    to: new PhoneNumber(number),
                    from: new PhoneNumber(_twilioSender),
                    body: message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
