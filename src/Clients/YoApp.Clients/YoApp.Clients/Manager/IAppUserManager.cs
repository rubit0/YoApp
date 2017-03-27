using System.Threading.Tasks;
using YoApp.Clients.Models;

namespace YoApp.Clients.Manager
{
    public interface IAppUserManager
    {
        AppUser User { get; }

        Task<AppUser> LoadUser();
        Task PersistUser();
        Task<bool> SyncDownAsync();
        Task<bool> SyncUpAsync();
        void InitUser(string phoneNumber);
    }
}