using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Clients.Helpers.EventArgs;
using YoApp.Clients.Models;
using YoApp.Clients.Pages.Chats;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.Manager
{
    public class ChatManager : IChatManager, IDisposable
    {
        public ObservableCollection<ChatPage> Pages { get; set; }

        private readonly IRealmStore _store;
        private readonly IChatService _chatService;

        public ChatManager(IRealmStore store, IChatService chatService)
        {
            _store = store;
            _chatService = chatService;
            Pages = new ObservableCollection<ChatPage>();

            _chatService.OnChatMessageReceived += OnChatMessageReceivedHandler;
        }

        private void OnChatMessageReceivedHandler(object sender, ChatMessageReceivedEventArgs e)
        {
            var chatBook = _store.Find<ChatBook>(e.Sender);
            if (chatBook == null)
                return;

            var chatMessage = new ChatMessage {
                DeliveryState = 3,
                IsIncomming = true,
                Message = e.Message
            };

            chatBook.PushMessage(_store, chatMessage);
        }

        public async Task OpenChat(Friend friend)
        {
            if (App.Current.MainPage.Navigation.ModalStack.Any())
                await App.Current.MainPage.Navigation.PopModalAsync();

            if (GetPageByFriend(friend) == null)
            {
                var chatBook = _store.Find<ChatBook>(friend.Key);
                Pages.Add(new ChatPage(friend, chatBook));
            }

            await App.Current.MainPage.Navigation.PopToRootAsync(false);
            await App.Current.MainPage.Navigation.PushAsync(GetPageByFriend(friend));
        }

        public ChatPage GetPageByFriend(Friend friend)
        {
            return Pages.FirstOrDefault(p => p.Friend.Equals(friend));
        }

        public void Dispose()
        {
            _chatService.OnChatMessageReceived -= OnChatMessageReceivedHandler;
        }
    }
}
