using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace YoApp.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
        }
    }
}
