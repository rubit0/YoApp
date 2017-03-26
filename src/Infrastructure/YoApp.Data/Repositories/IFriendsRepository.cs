using System.Collections.Generic;
using System.Threading.Tasks;
using YoApp.Data.Models;

namespace YoApp.Data.Repositories
{
    public interface IFriendsRepository
    {
        Task<ApplicationUser> FindByNameAsync(string name);
        Task<IEnumerable<ApplicationUser>> FindByNameRangeAsync(IEnumerable<string> names);
        Task<bool> IsMemberAsync(string name);
        Task<IEnumerable<string>> IsMemberRangeAsync(IEnumerable<string> names);
    }
}
