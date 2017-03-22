using System.Threading.Tasks;

namespace YoApp.Clients.Manager
{
    public interface IVerificationManager
    {
        /// <summary>
        /// Request the backend to start verification by sending us an verification code.
        /// </summary>
        /// <param name="countryCode">Clients local calling code</param>
        /// <param name="phonenumber">Clients phonenumber</param>
        /// <returns>Rrequest accepted by server?</returns>
        Task<bool> RequestVerificationCodeAsync(string countryCode, string phonenumber);

        /// <summary>
        /// Resolve verification to create an account on the backend
        /// and authenticate via tokens.
        /// </summary>
        /// <param name="verificationCode">Verification code from the server send via SMS.</param>
        /// <param name="phoneNumber">Country code and phonenumber concatenated</param>
        /// <param name="password">Password which should be a GIUD uppercase</param>
        /// <returns></returns>
        Task<bool> ResolveVerificationCodeAsync(string verificationCode, string phoneNumber, string password);
    }
}