using YoApp.Clients.Persistence;

namespace YoApp.Clients.Models
{
    public class AppUser : IKeyProvider
    {
        public string PhoneNumber { get; set; }
        public string Nickname { get; set; }
        public string Status { get; set; }

        public string Key => nameof(AppUser);
    }
}
