using Newtonsoft.Json;
using Rubito.SimpleFormsAuth;
using System;
using System.Net;
using System.Threading.Tasks;
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
        private readonly Uri _backendAddress;
        private readonly IKeyValueStore _keyValueStore;

        public AppUserManager()
        {
            _keyValueStore = App.StorageResolver.Resolve<IKeyValueStore>();
            _backendAddress = new Uri(App.Settings.Identity.Url, "Api/Account/");
        }

        /// <summary>
        /// Sync the users account properties upstream to the backend upstream.
        /// </summary>
        /// <returns>Was Sync successful</returns>
        public async Task<bool> SyncUpAsync()
        {
            var dto = new UpdatedAccountDto
            {
                Nickname = App.Settings.User.Nickname,
                StatusMessage = App.Settings.User.Status
            };

            var requestBody = JsonConvert.SerializeObject(dto);
            var request = new OAuth2BearerRequest("POST",
                _backendAddress,
                null,
                AuthenticationService.AuthAccount);

            request.SetRequestBody(requestBody);

            var response = await request.GetResponseAsync();
            await _keyValueStore.Persist();

            return (response.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// Sync from the backend downstream to this local user.
        /// </summary>
        /// <returns>Was Sync successful</returns>
        public async Task<bool> SyncDownAsync()
        {
            var request = new OAuth2BearerRequest("GET", _backendAddress, null, AuthenticationService.AuthAccount);
            var response = await request.GetResponseAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            var body = await response.GetResponseTextAsync();
            var dto = JsonConvert.DeserializeObject<UpdatedAccountDto>(body);

            if (dto == null)
                throw new NullReferenceException("Bad Json response when expecting an account dto.");

            App.Settings.User.Nickname = dto.Nickname;
            App.Settings.User.Status = dto.StatusMessage;
            await _keyValueStore.Persist();

            return true;
        }

        /// <summary>
        /// Creates the local AppUser. Only use this at the setup story.
        /// </summary>
        /// <param name="phoneNumber">Will be treated like an username</param>
        public async Task InitUserAsync(string phoneNumber)
        {
            App.Settings.User.PhoneNumber = phoneNumber;
            App.Settings.User.Status = App.Settings.Conventions.DefaultStatusMessage;

            await _keyValueStore.Persist();
        }
    }
}
