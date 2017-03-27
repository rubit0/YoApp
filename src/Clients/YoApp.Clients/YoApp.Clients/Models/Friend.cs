using YoApp.Clients.Persistence;
using YoApp.DataObjects.Users;

namespace YoApp.Clients.Models
{
    public class Friend : IKeyProvider
    {
        public string PhoneNumber { get; set; }
        public string Nickname { get; set; }
        public string StatusMessage { get; set; }
        public LocalContact LocalContact { get; set; }
        public string Key => PhoneNumber;

        public string DisplayName =>
            string.IsNullOrWhiteSpace(Nickname)
                ? LocalContact.DisplayName
                : Nickname;

        public static Friend CreateFromDto(UserDto dto)
        {
            if (dto == null)
                return null;

            return new Friend
            {
                PhoneNumber = dto.Username,
                Nickname = dto.Nickname,
                StatusMessage = dto.Status
            };
        }
    }
}