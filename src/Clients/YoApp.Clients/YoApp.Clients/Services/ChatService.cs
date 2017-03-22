using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Clients.Models;
using YoApp.Clients.Pages.Chats;
using YoApp.Clients.Persistence;

namespace YoApp.Clients.Services
{
    public class ChatService
    {
        public ObservableCollection<ChatPage> Pages { get; set; }

        private readonly IRealmStore _store;

        public ChatService()
        {
            _store = App.StorageResolver.Resolve<IRealmStore>();
            Pages = new ObservableCollection<ChatPage>();
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
