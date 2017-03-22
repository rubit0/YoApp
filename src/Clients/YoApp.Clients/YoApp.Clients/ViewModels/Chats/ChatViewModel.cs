using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using YoApp.Clients.Models;

namespace YoApp.Clients.ViewModels.Chats
{
    public class ChatViewModel
    {
        public string Name => _friend.DisplayName;
        public IList<ChatMessage> Messages => _chatBook?.Messages;
        public ICommand PostMessageCommand { get; set; }

        private readonly ChatBook _chatBook;
        private readonly Friend _friend;

        public ChatViewModel(Friend friend, ChatBook chatBook)
        {
            _friend = friend;
            _chatBook = chatBook;

            PostMessageCommand = new Command<string>(PostMessage);
        }

        private void PostMessage(string message)
        {
            var fake = new ChatMessage
            {
                Message = message,
                Date = DateTimeOffset.UtcNow,
                DeliveryState = 0,
                IsIncomming = false
            };

            _chatBook.PushMessage(fake);
        }
    }
}
