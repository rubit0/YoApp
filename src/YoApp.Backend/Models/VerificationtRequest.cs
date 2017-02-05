using System;
using YoApp.DataObjects.Verification;

namespace YoApp.Backend.Models
{
    public class VerificationtRequest
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public VerificationtRequest()
        {
            
        }

        public VerificationtRequest(string phoneNumber, TimeSpan duration, string verificationCode)
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

        public bool VerifyFromRequest(VerificationResolveDto reponse)
        {
            if (reponse == null)
                throw new ArgumentNullException();

            if (string.CompareOrdinal(this.PhoneNumber, reponse.PhoneNumber) != 0)
                return false;
            if (string.CompareOrdinal(this.VerificationCode, reponse.VerificationCode) != 0)
                return false;

            return true;
        }
    }
}
