using System;
using System.Collections.Generic;

namespace YoApp.Backend.Helper
{
    public interface IConfigurationService
    {
        TimeSpan VerificationDuration { get; }
        IEnumerable<int> ValidCountryCallCodes { get; }
        ConfigurationService.TwillioSettings Twillio { get; }
    }
}