using System;
using YoApp.DataObjects.Verification;

namespace YoApp.Data.Models
{
    public class VerificationToken
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Code { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public VerificationToken()
        {
        }
        
        public VerificationToken(string user, TimeSpan duration, string code)
        {
            this.User = user;
            this.Created = DateTime.Now;
            this.Expires = Created.Add(duration);
            this.Code = code;
        }

        public bool IsExpired()
        {
            return DateTime.Compare(Expires, DateTime.Now) < 0;
        }

        public bool ResolveToken(VerificationResolveDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            if (string.CompareOrdinal(this.User, dto.PhoneNumber) != 0)
                return false;
            if (string.CompareOrdinal(this.Code, dto.VerificationCode) != 0)
                return false;

            return true;
        }
    }
}
