using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YoApp.Backend.Models;
using YoApp.DataObjects.Verification;

namespace YoApp.Backend.Data.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<VerificationtRequestDto> VerificationtRequests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
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

            builder.Entity<VerificationtRequestDto>()
                .Property(vr => vr.PhoneNumber)
                .HasMaxLength(30);

            builder.Entity<VerificationtRequestDto>()
                .Property(vr => vr.VerificationCode)
                .HasMaxLength(8);

            base.OnModelCreating(builder);
        }
    }
}
