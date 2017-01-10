using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YoApp.Backend.DataObjects.Account
{
    public class InitialUserCreationForm
    {
        public int CountryCode { get; set; }
        public string PhoneNumber { get; set; }

        public string GetValidPhoneNumber()
        {
            var digitOnly = new Regex(@"[^\d]").Replace(PhoneNumber, "");
            var cleanNumber = digitOnly.Replace(" ", "");
            return $"+{CountryCode}{cleanNumber}";
        }
    }
}
