using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YoApp.Data.Extensions;
using YoApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using YoApp.Data;
using AspNet.Security.OpenIdConnect.Primitives;
using YoApp.Utils.Extensions;
using YoApp.Data.Repositories;
using YoApp.Identity.Helper;
using YoApp.Identity.Services;
using YoApp.Identity.Services.Interfaces;

namespace YoApp.Identity
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; private set; }

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

        public void ConfigureServices(IServiceCollection services)
        {
            //Persistence and connection strings.
            services.AddEntityFramework(Configuration["ConnectionStrings:DefaultConnection"]);

            //Identity persitence.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Identity configuration.
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

            //Add OpenIddict.
            services.AddOpenIddict()
                .AddEntityFrameworkCoreStores<ApplicationDbContext>()
                .AddMvcBinders()
                .EnableTokenEndpoint("/connect/token")
                .AllowPasswordFlow()
                .ChainIf(_environment.IsDevelopment(), (b) => 
                b.DisableHttpsRequirement()
                .AddEphemeralSigningKey());

            // Add framework services.
            services.AddMvc();

            //IoC
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddScoped<IVerificationTokensRepository, VerificationTokensRepository>();
            services.AddScoped<DataWorker>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            if (Configuration.IsLocalInstance() || _environment.IsDevelopment())
                services.AddSingleton<ISmsSender, DummyMessageService>();
            else
                services.AddSingleton<ISmsSender, TwilioMessageService>();
        }

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
