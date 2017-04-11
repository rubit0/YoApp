using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Identity persitence.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc();

            //Set App wide protection keyring.
            if (Configuration.IsLocalInstance())
            {
                services.ConfigureDataProtectionLocal("YoApp");
            }
            else
            {
                var section = Configuration.GetSection("Blobs:keyring");
                services.ConfigureDataProtectionOnAzure("YoApp", section["Account"], section["Secret"]);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            app.UseOAuthValidation();

            if (!Configuration.IsLocalInstance())
            {
                var dateProcProv = OwinDataProtectionFacade.CreateFromServiceProvider(serviceProvider);
                app.UseKatana(appBuilder => appBuilder
                    .MapSignalR()
                    .SetDataProtectionProvider(dateProcProv));
            }

            app.UseMvc();
        }
    }
}
