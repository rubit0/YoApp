
using System.Linq;
using Xamarin.Forms;
using YoApp.Clients.Models;
using YoApp.Clients.ViewModels.Chats;

namespace YoApp.Clients.Pages.Chats
{
    public partial class ChatPage : ContentPage
    {
        public Friend Friend { get; }

        public ChatPage(Friend friend, ChatBook chatBook)
        {
            InitializeComponent();
            Friend = friend;
            BindingContext = new ChatViewModel(Friend, chatBook);
        }

        protected override void OnAppearing()
        {
            ListView.SelectedItem = null;
            ListView.ScrollTo(ListView.ItemsSource.Cast<object>().Last(), ScrollToPosition.End, false);
        }

        protected override void OnDisappearing()
        {
            ListView.SelectedItem = null;
        }
    }
}
