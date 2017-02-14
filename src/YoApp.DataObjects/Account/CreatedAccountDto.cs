using System.ComponentModel.DataAnnotations;

namespace YoApp.DataObjects.Account
{
    public class CreatedAccountDto
    {
        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Phonenumber too long or short")]
        public string PhoneNumber { get; set; }

        public string Status { get; set; }
        public string Nickname { get; set; }
    }
}
