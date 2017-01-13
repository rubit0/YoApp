using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Backend.Models;

namespace YoApp.Backend.DataObjects.Account
{
    public class VerificationCode
    {
        public string Code { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public static VerificationCode CreateFromVerificationRequest(VerificationtRequest request)
        {
            return new VerificationCode
            {
                Code = request?.VerificationCode,
                CreationDate = request.CreationDate,
                ExpireDate = request.ExpireDate
            };
        }
    }
}
