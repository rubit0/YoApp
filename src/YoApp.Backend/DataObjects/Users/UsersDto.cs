using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YoApp.Backend.Models;

namespace YoApp.Backend.DataObjects.Users
{
    public class UsersDto
    {
        [Required]
        public string PhoneNumber { get; set; }
        public string Status { get; set; }

        public UsersDto()
        {
            
        }

        public UsersDto(string phoneNumber, string status)
        {
            this.PhoneNumber = phoneNumber;
            this.Status = status;
        }

        public bool IsValidModel()
        {
            return string.IsNullOrWhiteSpace(this.PhoneNumber);
        }

        public static IEnumerable<UsersDto> MapFromAppUsers(IEnumerable<ApplicationUser> users)
        {
            var dtos = new List<UsersDto>();

            foreach (var user in users)
                dtos.Add(new UsersDto(user.UserName, user.Status));

            return dtos;
        }
    }
}
