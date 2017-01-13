using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Backend.Models
{
    public class VerificationtRequest
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public string GenerateVerificationCode()
        {
            var rng = new Random();
            return $"{rng.Next(100, 1000)}-{rng.Next(100, 1000)}";
        }

        public bool IsExpired()
        {
            return ExpireDate < DateTime.Now;
        }

        public static VerificationtRequest CreateVerificationtRequest(string phoneNumber)
        {
            var request = new VerificationtRequest();

            request.PhoneNumber = phoneNumber;
            request.VerificationCode = request.GenerateVerificationCode();
            request.CreationDate = DateTime.Now;

            int duration = 500;
            int.TryParse(Startup.Configuration["VerificationCodes:Duration"], out duration);
            request.ExpireDate = request.CreationDate.Add(TimeSpan.FromSeconds(duration));

            return request;
        }
    }
}
