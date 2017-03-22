using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Chats;

namespace YoApp.Clients.Pages.Chats
{
    public partial class ChatsListPage : ContentPage, IPageService
    {
        public ChatsListPage()
        {
            InitializeComponent();
            BindingContext = new ChatsListViewModel(this);
        }

        protected override void OnAppearing()
        {
            ListView.SelectedItem = null;
        }
    }
}
