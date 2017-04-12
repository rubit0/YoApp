using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YoApp.Data.Extensions;
using YoApp.Data;
using YoApp.Data.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using YoApp.Core.Extensions;
using YoApp.Core.Models;
using YoApp.Friends.Helper;

namespace YoApp.Friends
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Persistence and connection strings.
            services.AddEntityFramework(Configuration["ConnectionStrings:DefaultConnection"]);

            //Identity persitence.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Set App-wide protection keyring.
            if (Configuration.IsLocalInstance())
            {
                services.ConfigureDataProtectionLocal("YoApp");
            }
            else
            {
                var section = Configuration.GetSection("Blobs:keyring");
                services.ConfigureDataProtectionOnAzure("YoApp", section["Account"], section["Secret"]);
            }

            //Framework services.
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));

            //IoC
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddScoped<IFriendsRepository, FriendsRepository>();
            services.AddScoped<IFriendsPersistence, Persistence>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            //Middleware.
            app.UseOAuthValidation();
            app.UseMvc();
        }
    }
}
