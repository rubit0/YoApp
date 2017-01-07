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

namespace YoApp.Backend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
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
                .AddDbContext<ApplicationDbContext>(
                    o => o.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
                    );

            //Identity persitence
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Identity configuration
            services.Configure<IdentityOptions>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 36;

                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = true;
            });

            // Add framework services.
            services.AddMvc();

            //IoC
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIdentity();
            app.UseMvc();
        }
    }
}
