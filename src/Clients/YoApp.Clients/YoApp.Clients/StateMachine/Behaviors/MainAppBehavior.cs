using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Manager;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.StateMachine.Behaviors
{
    /// <summary>
    /// Manage App LifeCycle events.
    /// </summary>
    public class MainAppBehavior : AppBehavior
    {
        private readonly IKeyValueStore _store;
        private readonly IAppUserManager _userManager;
        private readonly IContactsManager _contactsManager;
        private readonly IFriendsManager _friendsManager;
        private readonly IChatService _chatService;

        private bool _isHandlingStart;
        private bool _isHandlingSetup;

        public MainAppBehavior(IKeyValueStore store, IAppUserManager appUserManager, 
            IContactsManager contactsManager, IFriendsManager friendsManager, IChatService chatService)
        {
            _store = store;
            _userManager = appUserManager;
            _contactsManager = contactsManager;
            _friendsManager = friendsManager;
            _chatService = chatService;
        }

        public override async Task OnStart()
        {
            if(_isHandlingStart)
                return;

            _isHandlingStart = true;

            await _userManager.LoadUser();

            Device.BeginInvokeOnMainThread(() =>
                MessagingCenter.Send(this, MessagingEvents.AppLoadFinished, GetMainPage()));

            if (App.Settings.SetupFinished)
            {
                await _friendsManager.LoadFriendsFromStore();
                await _contactsManager.LoadContactsAsync();
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
                await _chatService.Connect();
            }

            _isHandlingStart = false;
        }

        public override async Task OnSleep()
        {
            await _store.Persist();
        }

        public override async Task OnResume()
        {
            if(!App.Settings.SetupFinished)
                return;

            if (await _contactsManager.LoadContactsAsync())
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
        }

        public override async Task OnSetupComplete()
        {
            if(_isHandlingSetup)
                return;

            await HandleSetup();
        }

        private async Task HandleSetup()
        {
            _isHandlingSetup = true;

            await App.Settings.Persist();
            await _contactsManager.LoadContactsAsync();
            await _friendsManager.ManageFriends(_contactsManager.Contacts);
            await _chatService.Connect();
            await _store.Persist();

            _isHandlingSetup = false;
        }

        private Page GetMainPage()
        {
            return (App.Settings.SetupFinished || ResourceKeys.IsDebug)
                ? new NavigationPage(new Pages.MainPage()) 
                : new NavigationPage(new Pages.Setup.WelcomePage());
        }
    }
}
