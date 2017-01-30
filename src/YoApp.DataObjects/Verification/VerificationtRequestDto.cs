using System;

namespace YoApp.DataObjects.Verification
{
    public class VerificationtRequestDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public VerificationtRequestDto()
        {
            
        }

        public VerificationtRequestDto(string phoneNumber, TimeSpan duration, string verificationCode)
        {
            this.PhoneNumber = phoneNumber;
            this.CreationDate = DateTime.Now;
            this.ExpireDate = CreationDate.Add(duration);
            this.VerificationCode = verificationCode;
        }

        public bool IsExpired()
        {
            return ExpireDate < DateTime.Now;
        }
    }
}
