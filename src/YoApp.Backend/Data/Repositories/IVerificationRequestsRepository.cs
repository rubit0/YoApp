using System.Threading.Tasks;
using YoApp.Backend.Models;

namespace YoApp.Backend.Data.Repositories
{
    public interface IVerificationRequestsRepository
    {
        VerificationtRequest FindVerificationtRequest(int id);
        Task<VerificationtRequest> FindVerificationtRequestAsync(int id);
        VerificationtRequest FindVerificationtRequestByPhone(string number);
        Task<VerificationtRequest> FindVerificationtRequestByPhoneAsync(string number);
        VerificationtRequest FindVerificationtRequestByCode(string code);
        Task<VerificationtRequest> FindVerificationtRequestByCodeAsync(string code);

        void AddVerificationRequest(VerificationtRequest request);
        Task AddVerificationRequestAsync(VerificationtRequest request);
        void RemoveVerificationRequest(int id);
        void RemoveVerificationRequestByPhone(string number);
    }
}
