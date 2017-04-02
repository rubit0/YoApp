﻿using System.Threading.Tasks;
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
        private readonly ChatService _chatService;
        private readonly IKeyValueStore _store;
        private readonly IAppUserManager _userManager;

        public LifeCycleState()
        {
            _contactsManager = App.Managers.Resolve<IContactsManager>();
            _friendsManager = App.Managers.Resolve<IFriendsManager>();
            _store = App.Persistence.Resolve<IKeyValueStore>();
            _userManager = App.Managers.Resolve<IAppUserManager>();
            _chatService = App.Services.Resolve<ChatService>();
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
            await _userManager.LoadUser();

            if (AuthenticationService.CanRequestToken())
                await AuthenticationService.RequestToken(true);

            if (await _contactsManager.LoadContactsAsync())
                await _friendsManager.ManageFriends(_contactsManager.Contacts);

            if (App.Settings.SetupFinished)
                await _chatService.Connect();
        }

        private async Task Sleep()
        {
            await _store.Persist();
        }

        private async Task Resume()
        {
            if (AuthenticationService.CanRequestToken())
                await AuthenticationService.RequestToken(true);

            if (await _contactsManager.LoadContactsAsync())
                await _friendsManager.ManageFriends(_contactsManager.Contacts);
        }
    }
}
