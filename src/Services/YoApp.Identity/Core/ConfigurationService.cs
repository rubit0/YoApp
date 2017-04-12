using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace YoApp.Identity.Core
{
    public class ConfigurationService : IConfigurationService
    {
        public TimeSpan VerificationDuration { get; private set; }
        public IEnumerable<int> CountriesBlackList { get; private set; }
        public TwillioSettings Twillio { get; private set; }

        private readonly IConfigurationRoot _configurationRoot;

        public ConfigurationService()
        {
            _configurationRoot = Startup.Configuration;
            LoadConfigurations();
            Twillio = new TwillioSettings(_configurationRoot);
        }

        public ConfigurationService(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
            LoadConfigurations();
            Twillio = new TwillioSettings(_configurationRoot);
        }

        private void LoadConfigurations()
        {
            {
                int duration = 200;
                int.TryParse(_configurationRoot["VerificationCodes:Duration"], out duration);
                VerificationDuration = TimeSpan.FromSeconds(duration);
            }

            CountriesBlackList = _configurationRoot
                .GetSection("CountryCodes")
                .Get<IEnumerable<CountryCode>>()
                .Select(cc => cc.Code)
                .ToList();
        }

        public class TwillioSettings
        {
            public string AccountSid { get; private set; }
            public string AuthToken { get; private set; }
            public string SenderPhoneNumber { get; private set; }

            private readonly IConfigurationRoot _configurationRoot;

            public TwillioSettings()
            {
                _configurationRoot = Startup.Configuration;
                LoadConfigurations();
            }

            public TwillioSettings(IConfigurationRoot configurationRoot)
            {
                _configurationRoot = configurationRoot;
                LoadConfigurations();
            }

            public void LoadConfigurations()
            {
                AccountSid = _configurationRoot["Twillio:Sid"];
                AuthToken = _configurationRoot["Twillio:Token"];
                SenderPhoneNumber = _configurationRoot["Twillio:Sender"];
            }
        }

        private class CountryCode
        {
            public int Code { get; set; }
            public string Country { get; set; }
        }
    }
}
