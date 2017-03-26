using System.Threading.Tasks;
using YoApp.Data.Models;

namespace YoApp.Data.Repositories
{
    public interface IVerificationTokensRepository
    {
        Task<VerificationToken> FindByIdAsync(int id);
        Task<VerificationToken> FindByUserAsync(string userName);
        Task<VerificationToken> FindByCodeAsync(string code);
        Task AddAsync(VerificationToken request);
        Task AddOrReplaceAsync(VerificationToken request);
        void Remove(VerificationToken token);
    }
}
