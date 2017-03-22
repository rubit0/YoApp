using System.Windows.Input;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Pages.Settings;
using YoApp.Clients.Services;

namespace YoApp.Clients.ViewModels.Settings
{
    public class SettingsPageViewModel
    {
        public string Nickname => App.Settings.User.Nickname;
        public string StatusMessage => App.Settings.User.Status;

        public ICommand OpenDebugMenuCommand { get; private set; }
        public ICommand OpenContactsListCommand { get; private set; }
        public ICommand OpenUserPage { get; private set; }

        public SettingsPageViewModel(IPageService pageService)
        {
            if (ResourceKeys.IsDebug)
                OpenDebugMenuCommand = new Command(async () =>
                    await pageService.Navigation.PushAsync(new DebugPage()));

            OpenContactsListCommand = new Command(async () =>
                    await pageService.Navigation.PushModalAsync(new ContactsSelectionPage()));

            OpenUserPage = new Command(async () =>
                    await pageService.Navigation.PushAsync(new UserDetailsPage()),
                () => AuthenticationService.AuthAccount != null);
        }
    }
}
