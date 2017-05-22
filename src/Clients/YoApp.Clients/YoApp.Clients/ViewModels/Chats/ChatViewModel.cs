using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.ViewModels.Chats
{
    public class ChatViewModel
    {
        public string Name => _friend.DisplayName;
        public IList<ChatMessage> Messages => _chatBook?.Messages;
        public ICommand PostMessageCommand { get; set; }

        private readonly ChatBook _chatBook;
        private readonly Friend _friend;
        private readonly IChatService _chatService;
        private readonly IRealmStore _realmStore;

        public ChatViewModel(Friend friend, ChatBook chatBook, IChatService chatService, IRealmStore realmStore)
        {
            _friend = friend;
            _chatBook = chatBook;
            _chatService = chatService;
            _realmStore = realmStore;

            PostMessageCommand = new Command<string>(PostMessage);
        }

        private async void PostMessage(string message)
        {
            var pending = new ChatMessage
            {
                Message = message,
                DeliveryState = 0,
                IsIncomming = false
            };

            _chatBook.PushMessage(_realmStore, pending);

            //TODO Handle failed delivery
            var result = await _chatService.SendMessage(_friend, message);
        }
    }
}
