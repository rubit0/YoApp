using System.Linq;
using Autofac;
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
            BindingContext = App.Container.Resolve<ChatViewModel>(
                new TypedParameter(typeof(Friend), friend),
                new TypedParameter(typeof(ChatBook), chatBook));
        }

        protected override void OnAppearing()
        {
            ListView.SelectedItem = null;

            var items = ListView.ItemsSource.Cast<object>();
            if(items.Any())
                ListView.ScrollTo(items.Last(), ScrollToPosition.End, false);
        }

        protected override void OnDisappearing()
        {
            ListView.SelectedItem = null;
        }
    }
}
