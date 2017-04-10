using System;
using Xamarin.Auth;

namespace YoApp.Clients.Core.Extensions
{
    /// <summary>
    /// Helper methods to get values from the Account properties without using magic strings.
    /// </summary>
    public static class AccountExtensions
    {
        #region Constants

        private const string PasswordProp = "password";
        private const string ExpirationProp = "expires_in";
        private const string Date = "date";
        private const string Type = "token_type";

        #endregion

        public static string Password(this Account account)
        {
            return account.Properties[PasswordProp];
        }

        public static string Tokentype(this Account account)
        {
            return account.Properties[Type];
        }

        public static TimeSpan ValidDuration(this Account account)
        {
            try
            {
                var seconds = int.Parse(account.Properties[ExpirationProp]);
                return TimeSpan.FromSeconds(seconds);
            }
            catch (FormatException)
            {
                return TimeSpan.Zero;
            }
        }

        public static DateTime Created(this Account account)
        {
            try
            {
                return DateTime.Parse(account.Properties[Date]);
            }
            catch (FormatException)
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ExpiresIn(this Account account)
        {
            try
            {
                var date = account.Created();

                return date.Add(account.ValidDuration());
            }
            catch (FormatException)
            {

                return DateTime.Now;
            }
        }

        public static bool IsValid(this Account account)
        {
            try
            {
                return (account.ExpiresIn() > DateTime.Now);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static int RemainingSeconds(this Account account)
        {
            return (int)(account.ExpiresIn() - DateTime.Now).TotalSeconds;
        }
    }
}
