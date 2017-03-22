using System.Threading.Tasks;
using YoApp.Data.Models;

namespace YoApp.Data.Repositories
{
    public interface IVerificationTokensRepository
    {
        VerificationToken FindById(int id);
        Task<VerificationToken> FindByIdAsync(int id);

        VerificationToken FindByUser(string userName);
        Task<VerificationToken> FindByUserAsync(string userName);

        VerificationToken FindByCode(string code);
        Task<VerificationToken> FindByCodeAsync(string code);

        void Add(VerificationToken request);
        Task AddAsync(VerificationToken request);

        void AddOrReplace(VerificationToken request);
        Task AddOrReplaceAsync(VerificationToken request);

        void RemoveById(int id);
        void RemoveByUser(string userName);
    }
}
