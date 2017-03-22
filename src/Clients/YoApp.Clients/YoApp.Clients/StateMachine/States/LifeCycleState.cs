using System.Threading.Tasks;
using YoApp.Clients.Helpers.EventArgs;
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
        private readonly IContactsManager _contactsManager;
        private readonly IFriendsManager _friendsManager;
        private readonly IKeyValueStore _keyValueStore;

        public LifeCycleState()
        {
            _contactsManager = App.Resolver.Resolve<IContactsManager>();
            _friendsManager = App.Resolver.Resolve<IFriendsManager>();
            _keyValueStore = App.StorageResolver.Resolve<IKeyValueStore>();
        }

        public async Task HandleState(Lifecycle state)
        {
            switch (state)
            {
                case Lifecycle.Start:
                    await Start();
                    break;
                case Lifecycle.Sleep:
                    await Sleep();
                    break;
                case Lifecycle.Resume:
                    await Resume();
                    break;
                default:
                    break;
            }
        }

        private async Task Start()
        {
            if (await AuthenticationService.CanRequest())
                await AuthenticationService.RequestToken(true);

            if (await _contactsManager.LoadContactsAsync())
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
        }

        private async Task Sleep()
        {
            await _keyValueStore.Persist();
        }

        private async Task Resume()
        {
            if (await AuthenticationService.CanRequest())
                await AuthenticationService.RequestToken(true);

            if (await _contactsManager.LoadContactsAsync())
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
        }
    }
}
