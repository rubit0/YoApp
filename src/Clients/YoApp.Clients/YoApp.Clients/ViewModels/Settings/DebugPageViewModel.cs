using Plugin.Connectivity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Forms;
using YoApp.Clients.Core.Extensions;
using YoApp.Clients.Forms;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Pages;
using YoApp.Clients.Pages.Setup;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.ViewModels.Settings
{
    public class DebugPageViewModel
    {
        public bool SetupComplete => App.Settings.SetupFinished;
        public string Nickname => AuthenticationService.AuthAccount?.Username ?? "NO USER";
        public string FriendsAmount => _friendsManager.Friends.Count().ToString();

        public string TokenType => AuthenticationService.AuthAccount?.Tokentype() ?? "NO USER";
        public DateTime TokenCreation => AuthenticationService.AuthAccount?.Created() ?? DateTime.MinValue;
        public DateTime TokenExpiration => AuthenticationService.AuthAccount?.ExpiresIn() ?? DateTime.MinValue;

        public string BackendEndpoint => App.Settings.Identity.Url.ToString();
        public int BackendPort => App.Settings.Identity.Port;
        public int BackendTimeout => App.Settings.Identity.TimeOut;

        public ICommand ExitCommand { get; private set; }
        public ICommand StartSetupCommand { get; private set; }
        public ICommand ClearSettingsCommand { get; private set; }
        public ICommand ClearSqlTablesCommand { get; private set; }
        public ICommand RefreshTokenCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }
        public ICommand PingBackendCommand { get; private set; }

        private readonly IPageService _pageService;
        private readonly IFriendsManager _friendsManager;
        private readonly IKeyValueStore _keyValueStore;
        private readonly IRealmStore _realmStore;

        public DebugPageViewModel(IPageService pageService, IKeyValueStore keyValueStore, IFriendsManager friendsManager, IRealmStore realmStore)
        {
            _pageService = pageService;
            _friendsManager = friendsManager;
            _realmStore = realmStore;
            _keyValueStore = keyValueStore;

            Initcommands();
        }

        private void Initcommands()
        {
            ExitCommand = new Command(() => Application.Current.MainPage = new NavigationPage(new MainPage()));
            StartSetupCommand = new Command(async () => await ResetSetup());
            ClearSettingsCommand = new Command(async () => await ClearSettings());
            ClearSqlTablesCommand = new Command(async () => await DropSqlTables());
            RefreshTokenCommand = new Command(async () => await RefreshToken(),
                () => AuthenticationService.AuthAccount != null);
            DeleteAccountCommand = new Command(async () => await DeleteLocalAccount(),
                () => AuthenticationService.AuthAccount != null);
            PingBackendCommand = new Command(async () => await PingBackend());
        }

        private async Task ResetSetup()
        {
            var result = await _pageService.DisplayAlert("Start Setup",
                "Restart the initial setup?\nThis will delete the local settings and account!",
                "Yes",
                "Cancel");

            if (result)
            {
                var accountStore = AccountStore.Create();

                if (AuthenticationService.AuthAccount != null)
                    accountStore.Delete(AuthenticationService.AuthAccount, App.Settings.ServiceId);

                App.Settings.ClearConfiguration();
                await _keyValueStore.RemoveAll<Friend>();
                await _keyValueStore.Persist();

                Application.Current.MainPage = new NavigationPage(new WelcomePage());
            }
        }

        private async Task ClearSettings()
        {
            var result = await _pageService.DisplayAlert("Reset Settings",
                "Do you want to reset all settings to default?",
                "Yes",
                "Cancel");

            if (result)
                App.Settings.ClearConfiguration();
        }

        private async Task DropSqlTables()
        {
            var result = await _pageService.DisplayAlert("Delete SQL Tables",
                "Are you sure to delete all Tables?",
                "Yes",
                "Cancel");

            if (result)
            {
                await _keyValueStore.RemoveAll<Friend>();
                await _realmStore.RemoveAll<ChatMessage>();
                await _realmStore.RemoveAll<ChatBook>();
            }
        }

        private async Task RefreshToken()
        {
            var result = await AuthenticationService.RequestToken(true);

            if (result)
            {
                await _pageService.DisplayAlert("Success!",
                    "An new token has been retrived and stored.",
                    "Ok");
            }
            else
            {
                await _pageService.DisplayAlert("Error",
                    "Something bad happend, no token.",
                    "Ok");
            }
        }

        private async Task DeleteLocalAccount()
        {
            var result = await _pageService.DisplayAlert("Delete Account?",
                    "Deleting the account will make new token requests invalid.",
                    "Ok", "Cancel");

            if (result)
            {
                var accountStore = AccountStore.Create();

                if (AuthenticationService.AuthAccount != null)
                    accountStore.Delete(AuthenticationService.AuthAccount, App.Settings.ServiceId);
            }
        }

        private async Task PingBackend()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await _pageService.DisplayAlert("Error", "Ping could not be run.\nYou are currently not online.", "Ok");
                return;
            }

            var result = await CrossConnectivity.Current.IsServiceOnlineAsync();
            await _pageService.DisplayAlert("Result", $"Connected: {result}", "Ok");
        }
    }
}
