using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using YoApp.Clients.Models;
using Rubito.SimpleFormsAuth;
using System.Collections.Generic;
using System.Net;
using YoApp.Clients.Helpers.EventArgs;

namespace YoApp.Clients.Services
{
    public class ChatService : IChatService
    {
        public event EventHandler<ChatMessageReceivedEventArgs> OnChatMessageReceived;

        private readonly Uri _baseAddress;
        private readonly Uri _sendMessageEndpoint;
        private HubConnection _connection;
        private IHubProxy _chatProxy;

        public ChatService()
        {
            _baseAddress = App.Settings.Chat.Url;
            _sendMessageEndpoint = new Uri(App.Settings.Chat.Url, "/messages/send");

            SetupSignalR();
        }

        private void OnTokenUpdatedHandler(object sender, string e)
        {
            if (_connection != null)
                _connection.Headers["Authorization"] = $"Bearer {e}";
        }

        public async Task<bool> Connect()
        {
            try
            {
                await _connection.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendMessage(Friend friend, string message)
        {
            var param = new Dictionary<string, string>
            {
                { "receiver", friend.PhoneNumber },
                { "message", message }
            };

            var request = new OAuth2BearerRequest("POST",
                _sendMessageEndpoint,
                param,
                AuthenticationService.AuthAccount);

            var response = await request.GetResponseAsync();
            return (response.StatusCode == HttpStatusCode.OK);
        }

        private void SetupSignalR()
        {
            _connection = new HubConnection(_baseAddress.ToString());

            var token = AuthenticationService.AuthAccount?.Properties["access_token"] ?? "";
            _connection.Headers.Add("Authorization", $"Bearer token");

            _chatProxy = _connection.CreateHubProxy("MainHub");
            _chatProxy.On("OnReceiveMessage", (string sender, string message) =>
            {
                ChatMessageReceivedHandler(sender, message);
            });

            AuthenticationService.OnTokenUpdated += OnTokenUpdatedHandler;
        }

        private void ChatMessageReceivedHandler(string sender, string message)
        {
            var args = new ChatMessageReceivedEventArgs {
                Sender = sender,
                Message = message
            };

            OnChatMessageReceived?.Invoke(this, args);
        }
    }
}
