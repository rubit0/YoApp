using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace YoApp.Data.Misc
{
    public class AppDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(LoadDefaultConnectionString());

            return new ApplicationDbContext(builder.Options);
        }

        private string LoadDefaultConnectionString()
        {
            var assembly = typeof(AppDbContextFactory).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("YoApp.Data.appsettings.json");
            if (stream == null)
                throw new InvalidOperationException("Could not locate 'appsettings.json'.");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var intermediate = JsonConvert.DeserializeObject<dynamic>(json);

                var connection = (string)intermediate["DefaultConnection"];
                if (string.IsNullOrEmpty(connection))
                    throw new InvalidOperationException($"{nameof(connection)}: No connection string found.");

                return connection;
            }
        }
    }
}
