using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using YoApp.Backend.Models;

namespace YoApp.Backend.DataObjects.Account
{
    public class InitialVerificationForm
    {
        [Required]
        public int CountryCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool CheckIsValidCountryCode()
        {
            var validCodesFromConfig = Startup.Configuration.GetSection("CountryCodes").Get<IEnumerable<CountryCodeOption>>();

            return validCodesFromConfig.Any(cc => cc.Code == CountryCode);
        }

        public string GetFormatedPhoneNumber()
        {
            var digitOnly = new Regex(@"[^\d]").Replace(PhoneNumber, "");
            var cleanNumber = digitOnly.Replace(" ", "");
            return $"{this.CountryCode}{this.PhoneNumber}";
        }

        class CountryCodeOption
        {
            public int Code { get; set; }
            public string Country { get; set; }
        }
    }
}
