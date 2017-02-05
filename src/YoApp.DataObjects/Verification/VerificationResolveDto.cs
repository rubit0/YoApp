using System;
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

        //public bool IsModelValid()
        //{
        //    if (string.IsNullOrWhiteSpace(this.PhoneNumber)
        //        || string.IsNullOrWhiteSpace(this.VerificationCode)
        //        || string.IsNullOrWhiteSpace(this.Password))
        //        return false;

        //    return true;
        //}

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(string.IsNullOrWhiteSpace(this.PhoneNumber))
        //        yield return new ValidationResult("You must provice a Phonenumber", new []{nameof(this.PhoneNumber)});
        //    if(string.IsNullOrWhiteSpace(this.VerificationCode))
        //        yield return new ValidationResult("VerificationCode is empty", new []{nameof(this.VerificationCode)});
        //    if(string.IsNullOrWhiteSpace(this.Password))
        //        yield return new ValidationResult("Password is empty", new []{nameof(this.Password)});
        //}
    }
}
