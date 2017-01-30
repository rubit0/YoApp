using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace YoApp.Backend.Helper
{
    public static class Settings
    {
        public static TimeSpan VerificationDuration { get; private set; }
        public static IEnumerable<int> ValidCountryCallCodes { get; private set; }

        static Settings()
        {
            LoadConfigurations();
        }

        private static void LoadConfigurations()
        {
            {
                int duration = 200;
                int.TryParse(Startup.Configuration["VerificationCodes:Duration"], out duration);
                VerificationDuration = TimeSpan.FromSeconds(duration);
            }

            ValidCountryCallCodes = Startup.Configuration
                .GetSection("CountryCodes")
                .Get<IEnumerable<CountryCode>>()
                .Select(cc => cc.Code)
                .ToList();
        }

        public static class TwillioSettings
        {
            public static string AccountSid { get; private set; }
            public static string AuthToken { get; private set; }
            public static string SenderPhoneNumber { get; private set; }

            static TwillioSettings()
            {
                AccountSid = Startup.Configuration["Twillio:Sid"];
                AuthToken = Startup.Configuration["Twillio:Token"];
                SenderPhoneNumber = Startup.Configuration["Twillio:Sender"];
            }
        }

        private class CountryCode
        {
            public int Code { get; set; }
            public string Country { get; set; }
        }
    }
}
