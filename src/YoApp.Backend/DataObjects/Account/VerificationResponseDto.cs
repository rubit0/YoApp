using System;
using System.ComponentModel.DataAnnotations;
using YoApp.Backend.Models;

namespace YoApp.Backend.DataObjects.Account
{
    public class VerificationResponseDto
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        [Required]
        [StringLength(36, MinimumLength = 32, ErrorMessage = "Password too short.")]
        public string Password { get; set; }

        public bool IsModelValid()
        {
            if (string.IsNullOrWhiteSpace(this.PhoneNumber)
                || string.IsNullOrWhiteSpace(this.VerificationCode)
                || string.IsNullOrWhiteSpace(this.Password))
                return false;

            return true;
        }

        public bool Verify(VerificationtRequest request)
        {
            if(request == null)
                throw new ArgumentNullException();

            if (string.CompareOrdinal(this.PhoneNumber, request.PhoneNumber) != 0)
                return false;
            if (string.CompareOrdinal(this.VerificationCode, request.VerificationCode) != 0)
                return false;

            return true;
        }
    }
}
