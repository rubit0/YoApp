using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Core.EventArgs;
using YoApp.Clients.Manager;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.StateMachine.States
{
    /// <summary>
    /// Manage App LifeCycle events.
    /// </summary>
    public class LifeCycleState
    {
        private readonly IKeyValueStore _store;
        private readonly IAppUserManager _userManager;
        private readonly IContactsManager _contactsManager;
        private readonly IFriendsManager _friendsManager;
        private readonly IChatService _chatService;

        private bool _ignoreLifeCycleEvents;

        public LifeCycleState(IKeyValueStore store, IAppUserManager appUserManager, 
            IContactsManager contactsManager, IFriendsManager friendsManager, IChatService chatService)
        {
            _store = store;
            _userManager = appUserManager;
            _contactsManager = contactsManager;
            _friendsManager = friendsManager;
            _chatService = chatService;
        }

        public async Task HandleState(Lifecycle state)
        {
            if(_ignoreLifeCycleEvents)
                return;

            switch (state)
            {
                case Lifecycle.Start:
                    _ignoreLifeCycleEvents = true;
                    await Start();
                    _ignoreLifeCycleEvents = false;
                    break;
                case Lifecycle.Sleep:
                    await Sleep();
                    break;
                case Lifecycle.Resume:
                    await Resume();
                    break;
                case Lifecycle.SetupCompleted:
                    _ignoreLifeCycleEvents = true;
                    await HandleSetup();
                    _ignoreLifeCycleEvents = false;
                    break;
                default:
                    return;
            }
        }

        private async Task Start()
        {
            await _userManager.LoadUser();

            if (AuthenticationService.CanRequestToken())
                await AuthenticationService.RequestToken(true);

            Device.BeginInvokeOnMainThread(() =>
                MessagingCenter.Send(this, MessagingEvents.AppLoadFinished, GetMainPage()));

            if (App.Settings.SetupFinished)
            {
                await _friendsManager.LoadFriends();
                await _contactsManager.LoadContactsAsync();
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
                await _chatService.Connect();
            }
        }

        private async Task Sleep()
        {
            await _store.Persist();
        }

        private async Task Resume()
        {
            if(!App.Settings.SetupFinished)
                return;
            
            if (AuthenticationService.CanRequestToken())
                await AuthenticationService.RequestToken(true);

            if (await _contactsManager.LoadContactsAsync())
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
        }

        private async Task HandleSetup()
        {
            await App.Settings.Persist();
            await _contactsManager.LoadContactsAsync();
            await _friendsManager.ManageFriends(_contactsManager.Contacts);
            await _chatService.Connect();
            await _store.Persist();
        }

        private Page GetMainPage()
        {
            return (App.Settings.SetupFinished || ResourceKeys.IsDebug)
                ? new NavigationPage(new Pages.MainPage()) 
                : new NavigationPage(new Pages.Setup.WelcomePage());
        }
    }
}
