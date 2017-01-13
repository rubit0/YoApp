using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Backend.Models;

namespace YoApp.Backend.DataObjects.Account
{
    public class VerificationChallenge
    {
        public string Code { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public static VerificationChallenge CreateFromVerificationRequest(VerificationtRequest request)
        {
            return new VerificationChallenge
            {
                Code = request?.VerificationCode,
                CreationDate = request.CreationDate,
                ExpireDate = request.ExpireDate
            };
        }
    }
}
