using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoApp.DataObjects.Verification
{

    public class VerificationChallengeDto
    {
        [Required]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "CountryCode too long or short")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 6, ErrorMessage = "Phonenumber too long or short")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Rerturns the CountryCode as an int.
        /// </summary>
        /// <returns></returns>
        public int CountryCodeToInt()
        {
            var result = 0;
            int.TryParse(this.CountryCode, out result);
            return result;
        }

        /// <summary>
        /// Returns CountryCode and PhoneNumber concatenated.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.CountryCode}{this.PhoneNumber}";
        }

        /// <summary>
        /// Get this Dto as a KeyValuePair.
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                {"CountryCode", this.CountryCode},
                {"PhoneNumber", this.PhoneNumber}
            };
        }
    }
}
