using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YoApp.Chat.Helpers;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using YoApp.Utils.Extensions;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //Set App wide protection keyring
            var accountName = Configuration.GetSection("Blob:keyring").GetValue<string>("Account");
            var secret = Configuration.GetSection("Blob:keyring").GetValue<string>("Secret");
            services.ConfigureDataProtectionOnAzure("YoApp", accountName, secret);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            app.UseAppBuilder((builder) => builder.MapSignalR());
            
            app.UseOAuthValidation();
            app.UseMvc();
        }
    }
}
