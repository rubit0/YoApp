using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoApp.DataObjects.Verification
{
    public class VerificationResolveDto
    {
        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Phonenumber too long or short")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6,ErrorMessage = "Verification too long or short")]
        public string VerificationCode { get; set; }

        [Required]
        [StringLength(36, MinimumLength = 32, ErrorMessage = "Password too long or short.")]
        public string Password { get; set; }

        /// <summary>
        /// Get this Dto as a KeyValuePair.
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                {"PhoneNumber", this.PhoneNumber},
                {"VerificationCode", this.VerificationCode},
                {"Password", this.Password}
            };
        }
    }
}
