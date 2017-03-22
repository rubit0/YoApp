using Xamarin.Forms;
using YoApp.Clients.Helpers;

namespace YoApp.Clients.Models
{
    public class LocalContact
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; }
        public string NormalizedPhoneNumber { get; }
        public bool IsValidPhoneNumber { get; }
        public bool IsMobile { get; }
        public string Label { get; set; }
        public FormattedString FormatedDisplayName => _formatted ?? (_formatted = BuildFormattedString());

        private FormattedString _formatted;

        public LocalContact(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            var parsed = PhoneNumberHelpers.ParsePhoneNumber(PhoneNumber);

            IsMobile = parsed.IsMobile;
            IsValidPhoneNumber = parsed.IsValid;
            NormalizedPhoneNumber = parsed.Normalized;
        }

        private FormattedString BuildFormattedString()
        {
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                return new FormattedString
                {
                    Spans =
                    {
                        new Span {Text = $"{FirstName} "},
                        new Span {Text = LastName, FontAttributes = FontAttributes.Bold}
                    }
                };
            }

            return new FormattedString
            {
                Spans =
                {
                    new Span {Text = $"{FirstName}", FontAttributes = FontAttributes.Bold}
                }
            };
        }

        public char GetSortFlag()
        {
            return !string.IsNullOrWhiteSpace(LastName)
                ? LastName[0]
                : DisplayName[0];
        }
    }
}
