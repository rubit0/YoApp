using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Data.Models;

namespace YoApp.Data.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetByName(string name);
        Task<ApplicationUser> GetByNameAsync(string name);

        IEnumerable<ApplicationUser> GetByNames(IEnumerable<string> names);
        Task<IEnumerable<ApplicationUser>> GetByNamesAsync(IEnumerable<string> names);

        Task<bool> IsMemberAsync(string name);

        IdentityResult Add(ApplicationUser user, string password);
        Task<IdentityResult> AddAsync(ApplicationUser user, string password);

        void UpdatePassword(ApplicationUser user, string password);
        Task UpdatePasswordAsync(ApplicationUser user, string password);
    }
}
