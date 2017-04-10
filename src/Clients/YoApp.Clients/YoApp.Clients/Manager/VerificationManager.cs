using ModernHttpClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using YoApp.Clients.Helpers;
using YoApp.Core.Dtos.Verification;

namespace YoApp.Clients.Manager
{
    /// <summary>
    /// Service to verify the AppUser on the intial setup flow
    /// </summary>
    public class VerificationManager : IVerificationManager
    {
        private readonly TimeSpan _timeOut;
        private readonly Uri _challengeAddress;
        private readonly Uri _resolveAddress;

        public VerificationManager(AppSettings settings)
        {
            _timeOut = TimeSpan.FromSeconds(settings.Identity.TimeOut);
            _challengeAddress = new Uri(settings.Identity.Url, "verification/request");
            _resolveAddress = new Uri(settings.Identity.Url, "verification/resolve");
        }

        /// <summary>
        /// Request the backend to start verification by sending us an verification code.
        /// </summary>
        /// <param name="countryCode">Clients local calling code</param>
        /// <param name="phonenumber">Clients phonenumber</param>
        /// <returns>Rrequest accepted by server?</returns>
        public async Task<bool> RequestVerificationCodeAsync(string countryCode, string phonenumber)
        {
            if(string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(phonenumber))
                throw new ArgumentNullException("You must provide an countrycode and phonenumber");

            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.Timeout = _timeOut;

                var dto = new VerificationChallengeDto
                {
                    CountryCode = countryCode,
                    PhoneNumber = phonenumber
                };
                var encodedContent = new FormUrlEncodedContent(dto.ToDictionary());

                try
                {
                    var response = await client.PostAsync(_challengeAddress, encodedContent);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Resolve verification to create an account on the backend
        /// and authenticate via tokens.
        /// </summary>
        /// <param name="verificationCode">Verification code from the server send via SMS.</param>
        /// <param name="phoneNumber">Country code and phonenumber concatenated</param>
        /// <param name="password">Password which should be a GIUD uppercase</param>
        /// <returns></returns>
        public async Task<bool> ResolveVerificationCodeAsync(string verificationCode, string phoneNumber, string password)
        {
            if (string.IsNullOrWhiteSpace(verificationCode) 
                || string.IsNullOrWhiteSpace(phoneNumber) 
                || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("You must provide all parameters");

            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.Timeout = _timeOut;

                var dto = new VerificationResolveDto
                {
                    VerificationCode = verificationCode,
                    PhoneNumber = phoneNumber,
                    Password = password
                };

                var encodedContent = new FormUrlEncodedContent(dto.ToDictionary());

                try
                {
                    var response = await client.PostAsync(_resolveAddress, encodedContent);

                    return response.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
