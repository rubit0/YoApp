using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<IEnumerable<string>> AreMemberAsync(IEnumerable<string> names);
    }
}
