using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Chats;

namespace YoApp.Clients.Pages.Chats
{
    public partial class FriendSelectionPage : ContentPage, IPageService
    {
        public FriendSelectionPage()
        {
            InitializeComponent();
            BindingContext = new FriendSelectionViewModel(this);
        }
    }
}
