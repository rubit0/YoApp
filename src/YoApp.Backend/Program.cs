using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace YoApp.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
#if DEBUG
                .UseUrls("http://0.0.0.0:5000")
#endif
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
