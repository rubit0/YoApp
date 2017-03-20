using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;
using YoApp.Backend.Data.EF;
using YoApp.Backend.Data.EF.Repositories;
using YoApp.Backend.Data.Repositories;
using YoApp.Backend.Helper;
using YoApp.Backend.Models;
using YoApp.Backend.Services;
using YoApp.Backend.Services.Interfaces;
using YoApp.Utils.Extensions;

namespace YoApp.Backend
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Persistence and connection strings
            services.AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
                    options.UseOpenIddict();
                });

            //Identity persitence
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Identity configuration
            services.Configure<IdentityOptions>(o =>
            {
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 32;

                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = false;

                o.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                o.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                o.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            //Set App wide protection keyring
            var accountName = Configuration.GetSection("Blob:keyring").GetValue<string>("Account");
            var secrect = Configuration.GetSection("Blob:keyring").GetValue<string>("Secret");
            services.ConfigureDataProtectionOnAzure("YoApp", accountName, secrect);

            //TODO Development configuration should go to StartupDevelopment.cs
            //Add OpenIddict
            if (_environment.IsDevelopment())
            {
                services.AddOpenIddict()
                    .AddEntityFrameworkCoreStores<ApplicationDbContext>()
                    .AddMvcBinders()
                    .EnableTokenEndpoint("/connect/token")
                    .AllowPasswordFlow()
                    .DisableHttpsRequirement()
                    .AddEphemeralSigningKey();
            }
            else
            {
                services.AddOpenIddict()
                    .AddEntityFrameworkCoreStores<ApplicationDbContext>()
                    .AddMvcBinders()
                    .EnableTokenEndpoint("/connect/token")
                    .AllowPasswordFlow();
            }

            // Add frameworks
            services.AddMvc();
            services.AddAutoMapper();

            //IoC
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVerificationRequestsRepository, VerificationRequestRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ISmsSender, TwilioMessageService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            app.UseOAuthValidation();
            app.UseOpenIddict();
            app.UseMvc();
        }
    }
}
