using System.Collections.Generic;
using System.Threading.Tasks;
using YoApp.Backend.Models;

namespace YoApp.Backend.Data.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetUserFromPhoneNumber(string phoneNumber);
        Task<ApplicationUser> GetUserFromPhoneNumberAsync(string phoneNumber);
        IEnumerable<ApplicationUser> GetUsersFromPhoneNumbers(IEnumerable<string> phoneNumbers);
        Task<IEnumerable<ApplicationUser>> GetUsersFromPhoneNumbersAsync(IEnumerable<string> phoneNumbers);
    }
}
