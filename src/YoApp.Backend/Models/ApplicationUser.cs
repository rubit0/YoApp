using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace YoApp.Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public string Status { get; set; } = "Greetings! I am using YoApp.";
    }
}
