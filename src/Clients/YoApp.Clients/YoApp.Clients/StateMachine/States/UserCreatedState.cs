using System.Threading.Tasks;
using YoApp.Clients.Manager;
using YoApp.Clients.Persistence;

namespace YoApp.Clients.StateMachine.States
{
    /// <summary>
    /// Manage the event where the setup has been finished.
    /// </summary>
    public class UserCreatedState
    {
        private readonly IContactsManager _contactsManager;
        private readonly IFriendsManager _friendsManager;
        private readonly IKeyValueStore _store;

        public UserCreatedState()
        {
            _store = App.Resolver.Resolve<IKeyValueStore>();
            _contactsManager = App.Resolver.Resolve<IContactsManager>();
            _friendsManager = App.Resolver.Resolve<IFriendsManager>();
        }

        public async Task Execute()
        {
            await App.Settings.Persist();
            await _friendsManager.ManageFriends(_contactsManager.Contacts);
            await App.ChatService.Connect();
        }
    }
}
