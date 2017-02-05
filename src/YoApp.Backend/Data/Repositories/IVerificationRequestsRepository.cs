using System.Threading.Tasks;
using YoApp.Backend.Models;
using YoApp.DataObjects.Verification;

namespace YoApp.Backend.Data.Repositories
{
    public interface IVerificationRequestsRepository
    {
        VerificationtRequest FindById(int id);
        Task<VerificationtRequest> FindByIdAsync(int id);

        VerificationtRequest FindByPhone(string number);
        Task<VerificationtRequest> FindByPhoneAsync(string number);

        VerificationtRequest FindByCode(string code);
        Task<VerificationtRequest> FindByCodeAsync(string code);

        void Add(VerificationtRequest request);
        Task AddAsync(VerificationtRequest request);

        void AddOrReplace(VerificationtRequest request);
        Task AddOrReplaceAsync(VerificationtRequest request);

        void RemoveById(int id);
        void RemoveByPhone(string number);
    }
}
