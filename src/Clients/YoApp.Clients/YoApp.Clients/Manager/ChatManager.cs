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
    public class ChatManager : IChatManager
    {
        public ObservableCollection<ChatPage> Pages { get; set; }

        private readonly IRealmStore _store;
        private readonly ChatService _chatService;

        public ChatManager()
        {
            _store = App.Persistence.Resolve<IRealmStore>();
            Pages = new ObservableCollection<ChatPage>();
            _chatService = App.Services.Resolve<ChatService>();

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

            chatBook.PushMessage(chatMessage);
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
    }
}
