using System.Threading.Tasks;
using YoApp.DataObjects.Verification;

namespace YoApp.Backend.Data.Repositories
{
    public interface IVerificationRequestsRepository
    {
        VerificationtRequestDto FindVerificationtRequest(int id);
        Task<VerificationtRequestDto> FindVerificationtRequestAsync(int id);

        VerificationtRequestDto FindVerificationtRequestByPhone(string number);
        Task<VerificationtRequestDto> FindVerificationtRequestByPhoneAsync(string number);

        VerificationtRequestDto FindVerificationtRequestByCode(string code);
        Task<VerificationtRequestDto> FindVerificationtRequestByCodeAsync(string code);

        void AddVerificationRequest(VerificationtRequestDto request);
        Task AddVerificationRequestAsync(VerificationtRequestDto request);

        void RemoveVerificationRequest(int id);
        void RemoveVerificationRequestByPhone(string number);
    }
}
