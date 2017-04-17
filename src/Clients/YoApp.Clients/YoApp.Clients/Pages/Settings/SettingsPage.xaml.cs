using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class SettingsTablePage : ContentPage, IPageService
    {
        public SettingsTablePage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            BindingContext = App.Container.Resolve<SettingsPageViewModel>(
                new TypedParameter(typeof(IPageService), this));
            DebugButton.IsVisible = ResourceKeys.IsDebug;
        }
    }
}
