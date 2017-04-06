using System.Threading.Tasks;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;
using YoApp.DataObjects.Account;

namespace YoApp.Clients.Manager
{
    /// <summary>
    /// Manages the AppUser on the db and sync with backend.
    /// </summary>
    public class AppUserManager : IAppUserManager
    {
        public AppUser User { get; private set; }
        private readonly IKeyValueStore _store;
        private readonly IAccountService _accountService;

        public AppUserManager(IKeyValueStore store, IAccountService accountService)
        {
            _store = store;
            _accountService = accountService;
        }

        public async Task<AppUser> LoadUser()
        {
            User = await _store.Get<AppUser>(nameof(AppUser));
            return User;
        }

        public async Task PersistUser()
        {
            if (User == null)
                return;

            await _store.Insert(User);
            await _store.Persist();
        }

        /// <summary>
        /// Sync the users account properties upstream to the backend upstream.
        /// </summary>
        /// <returns>Was Sync successful</returns>
        public async Task<bool> SyncUpAsync()
        {
            var dto = new UpdatedAccountDto
            {
                Nickname = this.User.Nickname,
                StatusMessage = this.User.Status
            };

            var result = await _accountService.SyncUpAsync(dto);
            if (result == false)
                return false;

            await PersistUser();
            return true;
        }

        /// <summary>
        /// Sync from the backend downstream to this local user.
        /// </summary>
        /// <returns>Was Sync successful</returns>
        public async Task<bool> SyncDownAsync()
        {
            var dto = await _accountService.SyncDownAsync();
            if (dto == null)
                return false;

            User.Nickname = dto.Nickname;
            User.Status = dto.StatusMessage;
            await PersistUser();

            return true;
        }

        /// <summary>
        /// Creates the local AppUser. Only use this at the setup story.
        /// </summary>
        /// <param name="phoneNumber">Will be treated like an username</param>
        public void InitUser(string phoneNumber)
        {
            User = new AppUser
            {
                Nickname = "",
                PhoneNumber = phoneNumber,
                Status = App.Settings.Conventions.DefaultStatusMessage
            };
        }
    }
}
