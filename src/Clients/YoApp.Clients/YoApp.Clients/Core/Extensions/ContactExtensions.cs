using Plugin.Contacts.Abstractions;

namespace YoApp.Clients.Core.Extensions
{
    public static class ContactExtensions
    {
        public static char GetSortFlag(this Contact source)
        {
            return !string.IsNullOrWhiteSpace(source.LastName)
                ? source.LastName[0]
                : source.FirstName[0];
        }
    }
}
