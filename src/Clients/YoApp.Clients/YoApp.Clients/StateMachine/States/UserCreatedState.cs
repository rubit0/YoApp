using System.Threading.Tasks;
using YoApp.Clients.Manager;
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
        private readonly IChatService _chatService;

        public UserCreatedState(IContactsManager contactsManager, IFriendsManager friendsManager, IChatService chatService)
        {
            _contactsManager = contactsManager;
            _friendsManager = friendsManager;
            _chatService = chatService;
        }

        public async Task Execute()
        {
            await App.Settings.Persist();
            await _friendsManager.ManageFriends(_contactsManager.Contacts);
            await _chatService.Connect();
        }
    }
}
