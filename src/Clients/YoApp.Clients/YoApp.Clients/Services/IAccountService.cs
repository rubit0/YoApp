using System.Threading.Tasks;
using YoApp.DataObjects.Account;

namespace YoApp.Clients.Services
{
    public interface IAccountService
    {
        Task<UpdatedAccountDto> SyncDownAsync();
        Task<bool> SyncUpAsync(UpdatedAccountDto dto);
    }
}