using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YoApp.Chat.Core;
using YoApp.Core.Extensions;
using YoApp.Core.Models;

namespace YoApp.Chat
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Identity persitence.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders();

            //Framework services.
            services.AddMvc();
            services.AddSignalR();

            //Set App-wide protection keyring.
            if (Configuration.IsLocalInstance())
            {
                services.ConfigureDataProtectionLocal("YoApp");
            }
            else
            {
                var section = Configuration.GetSection("Blobs:keyring");
                services.ConfigureDataProtectionForAspNet2OnAzure("YoApp", section["Account"], section["Secret"]);
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            //Currently broken when deployed to Azure
            //loggerFactory.AddAzureWebAppDiagnostics();

            //Middleware.
            app.UseOAuthValidation();
            app.UseWebSockets();
            app.UseSignalR();
            app.UseMvc();
        }
    }
}
