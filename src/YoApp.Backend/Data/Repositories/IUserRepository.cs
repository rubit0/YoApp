using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Backend.Models;

namespace YoApp.Backend.Data.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetUser(string name);
        Task<ApplicationUser> GetUserAsync(string name);

        IEnumerable<ApplicationUser> GetUsers(IEnumerable<string> names);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync(IEnumerable<string> names);

        IdentityResult AddUser(ApplicationUser user, string password);
        Task<IdentityResult> AddUserAsync(ApplicationUser user, string password);

        void UpdateUserPassword(ApplicationUser user, string password);
        Task UpdateUserPasswordAsync(ApplicationUser user, string password);
    }
}
