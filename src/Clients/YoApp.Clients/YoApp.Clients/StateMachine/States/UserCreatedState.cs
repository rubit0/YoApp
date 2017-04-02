using System.Threading.Tasks;
using YoApp.Clients.Manager;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.StateMachine.States
{
    /// <summary>
    /// Manage the event where the setup has been finished.
    /// </summary>
    public class UserCreatedState
    {
        private readonly IContactsManager _contactsManager;
        private readonly IFriendsManager _friendsManager;
        private readonly ChatService _chatService;
        private readonly IKeyValueStore _store;

        public UserCreatedState()
        {
            _store = App.Managers.Resolve<IKeyValueStore>();
            _contactsManager = App.Managers.Resolve<IContactsManager>();
            _friendsManager = App.Managers.Resolve<IFriendsManager>();
            _chatService = App.Services.Resolve<ChatService>();
        }

        public async Task Execute()
        {
            await App.Settings.Persist();
            await _friendsManager.ManageFriends(_contactsManager.Contacts);
            await _chatService.Connect();
        }
    }
}
