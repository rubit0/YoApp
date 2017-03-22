using System.Threading.Tasks;
using YoApp.Clients.Manager;

namespace YoApp.Clients.StateMachine.States
{
    /// <summary>
    /// Manage the event where the setup has been finished.
    /// </summary>
    public class UserCreatedState
    {
        private readonly IContactsManager _contactsManager;
        private readonly IFriendsManager _friendsManager;

        public UserCreatedState()
        {
            _contactsManager = App.Resolver.Resolve<IContactsManager>();
            _friendsManager = App.Resolver.Resolve<IFriendsManager>();
        }

        public async Task Execute()
        {
            await _friendsManager.ManageFriends(_contactsManager.Contacts);
        }
    }
}
