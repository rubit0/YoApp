using PhoneNumbers;

namespace YoApp.Clients.Helpers
{
    public static class PhoneNumberHelpers
    {
        private static readonly PhoneNumberUtil PhoneNumberUtil;
        private static readonly string RegionName;

        static PhoneNumberHelpers()
        {
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
            RegionName = System.Globalization.RegionInfo
                .CurrentRegion
                .TwoLetterISORegionName;
        }

        /// <summary>
        /// Check if the Phone Number is valid and properly format it.
        /// </summary>
        /// <param name="phoneNumber">Source to parse.</param>
        /// <returns>Parsed result.</returns>
        public static ParsedPhoneNumber ParsePhoneNumber(string phoneNumber)
        {
            try
            {
                var parsed = PhoneNumberUtil.Parse(phoneNumber, RegionName);

                return new ParsedPhoneNumber
                {
                    Normalized = $"{parsed.CountryCode}{parsed.NationalNumber}",
                    IsValid = PhoneNumberUtil.IsValidNumber(parsed),
                    IsMobile = PhoneNumberUtil.GetNumberType(parsed) == PhoneNumberType.MOBILE
                };
            }
            catch (NumberParseException)
            {
                return new ParsedPhoneNumber
                {
                    IsValid = false
                };
            }
        }
    }
}