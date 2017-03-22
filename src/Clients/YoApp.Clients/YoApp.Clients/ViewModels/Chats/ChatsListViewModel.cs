using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Pages.Chats;

namespace YoApp.Clients.ViewModels.Chats
{
    public class ChatsListViewModel
    {
        public ObservableCollection<ChatPage> Pages => App.ChatService.Pages;
        public Command OpenFriendsListCommand { get; private set; }
        public Command OpenChatCommand { get; private set; }

        private bool _canOpenFriend = true;
        private bool _canOpenChat = true;
        private readonly IPageService _pageService;

        public ChatsListViewModel(IPageService pageService)
        {
            _pageService = pageService;

            OpenFriendsListCommand = new Command(async
                () => await OpenFriendsListPage(),
                () => _canOpenFriend);

            OpenChatCommand = new Command(async
                (o) => await OpenChatPage(o),
                (o) => _canOpenChat);
        }

        private async Task OpenFriendsListPage()
        {
            _canOpenFriend = false;
            OpenFriendsListCommand.ChangeCanExecute();

            await _pageService.Navigation.PushModalAsync(new NavigationPage(new FriendSelectionPage()));

            _canOpenFriend = true;
            OpenFriendsListCommand.ChangeCanExecute();
        }

        private async Task OpenChatPage(object item)
        {
            var chatPage = item as ChatPage;
            if (chatPage != null)
            {
                _canOpenChat = false;
                await App.ChatService.OpenChat(chatPage.Friend);
                _canOpenChat = true;
            }
        }
    }
}
