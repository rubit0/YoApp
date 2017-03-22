using System.Threading.Tasks;

namespace YoApp.Clients.Manager
{
    public interface IAppUserManager
    {
        Task<bool> SyncDownAsync();
        Task<bool> SyncUpAsync();
        Task InitUserAsync(string phoneNumber);
    }
}