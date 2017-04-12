using System;
using YoApp.Core.Models;
using YoApp.Dtos.Verification;

namespace YoApp.Identity.Core
{
    public static class VerificationTokenExtensions
    {
        public static bool ResolveTokenWithDto(this VerificationToken token, VerificationResolveDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            if (string.CompareOrdinal(token.User, dto.PhoneNumber) != 0
                || string.CompareOrdinal(token.Code, dto.VerificationCode) != 0)
                return false;

            return true;
        }
    }
}
