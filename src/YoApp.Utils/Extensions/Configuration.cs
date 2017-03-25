using Microsoft.Extensions.Configuration;

namespace YoApp.Utils.Extensions
{
    public static class Configuration
    {
        public static bool IsLocalInstance(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("RUN_LOCAL");
        }
    }
}
