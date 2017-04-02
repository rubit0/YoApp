using System.Windows.Input;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Settings;
using YoApp.Clients.Services;

namespace YoApp.Clients.ViewModels.Settings
{
    public class SettingsPageViewModel
    {
        public string Nickname => _userManager.User?.Nickname ?? "";
        public string StatusMessage => _userManager.User?.Status ?? "";

        public ICommand OpenDebugMenuCommand { get; private set; }
        public ICommand OpenContactsListCommand { get; private set; }
        public ICommand OpenUserPage { get; private set; }

        private readonly IAppUserManager _userManager;

        public SettingsPageViewModel(IPageService pageService)
        {
            _userManager = App.Managers.Resolve<IAppUserManager>();

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
