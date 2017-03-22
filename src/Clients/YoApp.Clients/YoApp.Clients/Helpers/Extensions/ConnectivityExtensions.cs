using Plugin.Connectivity.Abstractions;
using System.Threading.Tasks;

namespace YoApp.Clients.Helpers.Extensions
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
            var url = $"http://{App.Settings.Backend.Host}";
            var status = await current
                .IsRemoteReachable(url,
                App.Settings.Backend.Port,
                App.Settings.Backend.TimeOut);

            return status;
        }
    }
}
