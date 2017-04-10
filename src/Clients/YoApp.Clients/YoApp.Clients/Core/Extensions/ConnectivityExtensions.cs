using System.Threading.Tasks;
using Plugin.Connectivity.Abstractions;

namespace YoApp.Clients.Core.Extensions
{
    public static class ConnectivityExtensions
    {
        /// <summary>
        /// Check if the backend is online or reachable.
        /// </summary>
        /// <param name="current">Connectivity service</param>
        /// <returns>Is backend onine?</returns>
        public static async Task<bool> IsServiceOnlineAsync(this IConnectivity current)
        {
            var url = $"{App.Settings.Identity.Url.Scheme}://{App.Settings.Identity.Url.Host}/";
            var status = await current
                .IsRemoteReachable(url,
                App.Settings.Identity.Port,
                App.Settings.Identity.TimeOut);

            return status;
        }
    }
}
