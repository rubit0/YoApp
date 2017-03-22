using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class SettingsTablePage : ContentPage, IPageService
    {
        public SettingsTablePage()
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel(this);
        }
    }
}
