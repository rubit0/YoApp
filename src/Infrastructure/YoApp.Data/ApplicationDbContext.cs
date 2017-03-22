using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YoApp.Data.Models;
using Microsoft.Extensions.DependencyInjection;

namespace YoApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<VerificationToken> VerificationTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .Property(au => au.Status)
                .HasMaxLength(30);

            builder.Entity<ApplicationUser>()
                .Property(au => au.Nickname)
                .HasMaxLength(20);

            builder.Entity<VerificationToken>()
                .Property(vt => vt.User)
                .HasMaxLength(30);

            builder.Entity<VerificationToken>()
                .Property(vt => vt.Code)
                .HasMaxLength(6);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseOpenIddict();
        }
    }
}
