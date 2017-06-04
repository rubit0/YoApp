using Plugin.Connectivity;
using Rubito.SimpleFormsAuth;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;
using YoApp.Clients.Core.Extensions;

namespace YoApp.Clients.Services
{
    /// <summary>
    /// Service for fetching and updating the OAuth2 authentication token for the local account.
    /// </summary>
    public static class AuthenticationService
    {
        public static event EventHandler<string> OnTokenUpdated;
        public static Account AuthAccount { get; private set; }

        private static readonly AccountStore AccountStore;
        private static readonly Uri TokenEndpoint;

        static AuthenticationService()
        {
            TokenEndpoint = new Uri($"{App.Settings.Identity.Url}connect/token");
            AccountStore = AccountStore.Create();
            AuthAccount = AccountStore
                .FindAccountsForService(App.Settings.ServiceId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Request a new bearer token and store it on the local Account.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns success state</returns>
        public static async Task<bool> RequestToken(string username, string password)
        {
            var authenticator = new OAuth2PasswordCredentialsAuthenticator(TokenEndpoint);
            authenticator.SetCredentials(username, password);

            try
            {
                var account = await authenticator.SignInAsync();
                if (account == null)
                    return false;

                account.Properties.Add("password", password);
                account.Properties.Add("date", DateTime.Now.ToString());

                await AccountStore.SaveAsync(account, App.Settings.ServiceId);

                AuthAccount = account;
                OnTokenUpdated?.Invoke(null, (string)account.Properties["access_token"]);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Request a new bearer token by using the credentials of an existing local account.
        /// </summary>
        /// <param name="force">Request a new token regardless if current token is valid.</param>
        /// <returns>Returns success state</returns>
        public static async Task<bool> RequestToken(bool force = false)
        {
            if (AuthAccount == null)
                throw new Exception("You must not call this method before the User has been initiated");

            if (AuthAccount.IsValid() && !force)
                return true;

            return await RequestToken(AuthAccount.Username, AuthAccount.Password());
        }

        /// <summary>
        /// Check if a token can be requested.
        /// </summary>
        /// <returns>Is a request possible?</returns>
        public static bool CanRequestToken()
        {
            return App.Settings.SetupFinished
                   && AuthAccount != null
                   && CrossConnectivity.Current.IsConnected;
        }

        /// <summary>
        /// Generate a new GUID style password by a random seed
        /// </summary>
        /// <returns>Random password</returns>
        public static string GeneratePassword()
        {
            var guidBytes = new byte[16];
            new Random().NextBytes(guidBytes);

            return new Guid(guidBytes).ToString();
        }
    }
}