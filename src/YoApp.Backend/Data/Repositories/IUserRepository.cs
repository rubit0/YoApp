using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Backend.Models;

namespace YoApp.Backend.Data.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetByUsername(string name);
        Task<ApplicationUser> GetByUsernameAsync(string name);

        IEnumerable<ApplicationUser> GetByUsernames(IEnumerable<string> names);
        Task<IEnumerable<ApplicationUser>> GetByUsernamesAsync(IEnumerable<string> names);

        Task<bool> IsMemberAsync(string name);

        IdentityResult Add(ApplicationUser user, string password);
        Task<IdentityResult> AddAsync(ApplicationUser user, string password);

        void UpdatePassword(ApplicationUser user, string password);
        Task UpdatePasswordAsync(ApplicationUser user, string password);
    }
}
