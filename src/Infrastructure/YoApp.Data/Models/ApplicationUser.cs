using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace YoApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }
        public string Status { get; set; } = "Greetings! I am using YoApp.";
    }
}
