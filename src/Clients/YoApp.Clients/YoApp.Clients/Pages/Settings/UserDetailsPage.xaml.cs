
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class UserDetailsPage : ContentPage, IPageService
    {
        public UserDetailsPage()
        {
            InitializeComponent();
            BindingContext = new UserDetailsPageViewModel(this);
        }
    }
}
