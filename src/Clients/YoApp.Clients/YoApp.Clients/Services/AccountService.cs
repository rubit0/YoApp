using Newtonsoft.Json;
using Rubito.SimpleFormsAuth;
using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using YoApp.Dtos.Account;

namespace YoApp.Clients.Services
{
    public class AccountService : IAccountService
    {
        private readonly Uri _backendAddress;
        private readonly IUserDialogs _userDialogs;

        public AccountService(IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;
            _backendAddress = new Uri(App.Settings.Identity.Url, "account");
        }

        /// <summary>
        /// Sync from the backend downstream to this local user.
        /// </summary>
        /// <returns>Accoutn object from backend.</returns>
        public async Task<UpdatedAccountDto> SyncDownAsync()
        {
            var request = new OAuth2BearerRequest("GET",
                _backendAddress,
                null,
                AuthenticationService.AuthAccount);

            try
            {
                var response = await request.GetResponseAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return null;

                var text = await response.GetResponseTextAsync();
                return JsonConvert.DeserializeObject<UpdatedAccountDto>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Sync the users account properties upstream to the backend upstream.
        /// </summary>
        /// <returns>Was Sync successful</returns>
        public async Task<bool> SyncUpAsync(UpdatedAccountDto dto)
        {
            var requestBody = JsonConvert.SerializeObject(dto);
            var request = new OAuth2BearerRequest("POST",
                _backendAddress,
                null,
                AuthenticationService.AuthAccount);

            request.SetRequestBody(requestBody);

            try
            {
                var response = await request.GetResponseAsync();
                var result = response.StatusCode == System.Net.HttpStatusCode.OK;
                _userDialogs.Toast(result ? "Updated user account." : "Failed to update user account.");
                
                return result;
            }
            catch (Exception)
            {
                _userDialogs.ShowError("Connection to service failed.");

                return false;
            }
        }
    }
}
