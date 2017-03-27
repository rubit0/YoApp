using Newtonsoft.Json;
using Rubito.SimpleFormsAuth;
using System;
using System.Threading.Tasks;
using YoApp.DataObjects.Account;

namespace YoApp.Clients.Services
{
    public class AccountService
    {
        private static readonly Uri _backendAddress;

        static AccountService()
        {
            _backendAddress = new Uri(App.Settings.Identity.Url, "account/");
        }

        /// <summary>
        /// Sync from the backend downstream to this local user.
        /// </summary>
        /// <returns>Accoutn object from backend.</returns>
        public static async Task<UpdatedAccountDto> SyncDownAsync()
        {
            var request = new OAuth2BearerRequest("GET",
                _backendAddress,
                null,
                AuthenticationService.AuthAccount);

            var response = await request.GetResponseAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            var text = await response.GetResponseTextAsync();
            return JsonConvert.DeserializeObject<UpdatedAccountDto>(text);
        }

        /// <summary>
        /// Sync the users account properties upstream to the backend upstream.
        /// </summary>
        /// <returns>Was Sync successful</returns>
        public static async Task<UpdatedAccountDto> SyncUpAsync(UpdatedAccountDto dto)
        {
            var requestBody = JsonConvert.SerializeObject(dto);
            var request = new OAuth2BearerRequest("POST",
                _backendAddress,
                null,
                AuthenticationService.AuthAccount);

            request.SetRequestBody(requestBody);

            var response = await request.GetResponseAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            var text = await  response.GetResponseTextAsync();
            return JsonConvert.DeserializeObject<UpdatedAccountDto>(text);
        }
    }
}
