using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoApp.DataObjects.Verification
{

    public class VerificationChallengeDto : IValidatableObject
    {
        [Required]
        public int CountryCode { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        /// <summary>
        /// Returns CountryCode and PhoneNumber concatenated
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.CountryCode}{this.PhoneNumber}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(this.CountryCode < 1 || this.CountryCode > 999)
                yield return new ValidationResult("Invalid Country code", new []{nameof(this.CountryCode)});

            if (this.PhoneNumber < 5)
                yield return new ValidationResult("PhoneNumber is too short", new []{nameof(this.PhoneNumber)});
        }
    }
}
