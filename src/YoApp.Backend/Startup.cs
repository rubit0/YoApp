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
using YoApp.Backend.Models;
using YoApp.Backend.Services;
using YoApp.Backend.Services.Interfaces;

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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
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
                o.Password.RequireLowercase = false;
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 32;

                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = true;
            });

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

            // Add framework services.
            services.AddMvc();
            services.AddSignalR(o =>
            {
                if (_environment.IsDevelopment())
                    o.Hubs.EnableDetailedErrors = true;

                if(_environment.IsProduction())
                    o.Hubs.EnableJavaScriptProxies = false;
            });

            //IoC
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVerificationRequestsRepository, VerificationRequestRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IMessageSender, TwilioMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseOAuthValidation();
            app.UseOpenIddict();
            app.UseMvc();
            app.UseWebSockets();
            app.UseSignalR();
        }
    }
}
