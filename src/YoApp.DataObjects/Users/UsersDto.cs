using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoApp.DataObjects.Users
{
    public class UsersDto
    {
        [Required]
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public string Nickname { get; set; }
    }
}
