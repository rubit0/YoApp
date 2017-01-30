using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace YoApp.DataObjects.Verification
{
    public class VerificationFormDto
    {
        [Required]
        public int CountryCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool IsModelValid()
        {
            return this.CountryCode > 0 && !string.IsNullOrWhiteSpace(PhoneNumber);
        }

        public string GetFormatedPhoneNumber()
        {
            var digitOnly = new Regex(@"[^\d]").Replace(PhoneNumber, "");
            var cleanNumber = digitOnly.Replace(" ", "");
            return $"{this.CountryCode}{this.PhoneNumber}";
        }
    }
}
