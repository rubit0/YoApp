using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YoApp.Backend.Data.Repositories;
using YoApp.DataObjects.Verification;

namespace YoApp.Backend.Data.EF.Repositories
{
    public class VerificationRequestRepository : IVerificationRequestsRepository
    {
        private readonly ApplicationDbContext _context;

        public VerificationRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public VerificationtRequestDto FindVerificationtRequest(int id)
        {
            return _context
                .VerificationtRequests
                .SingleOrDefault(vr => vr.Id == id);
        }

        public async Task<VerificationtRequestDto> FindVerificationtRequestAsync(int id)
        {
            return await _context
                .VerificationtRequests
                .SingleOrDefaultAsync(vr => vr.Id == id);
        }

        public VerificationtRequestDto FindVerificationtRequestByPhone(string number)
        {
            return _context
                .VerificationtRequests
                .SingleOrDefault(vr => vr.PhoneNumber == number);
        }

        public async Task<VerificationtRequestDto> FindVerificationtRequestByPhoneAsync(string number)
        {
            return await _context
                .VerificationtRequests
                .SingleOrDefaultAsync(vr => vr.PhoneNumber == number);
        }

        public VerificationtRequestDto FindVerificationtRequestByCode(string code)
        {
            return _context
                .VerificationtRequests
                .SingleOrDefault(vr => vr.VerificationCode == code);
        }

        public async Task<VerificationtRequestDto> FindVerificationtRequestByCodeAsync(string code)
        {
            return await _context
                .VerificationtRequests
                .SingleOrDefaultAsync(vr => vr.VerificationCode == code);
        }

        public void AddVerificationRequest(VerificationtRequestDto request)
        {
            if(request == null)
                return;

            _context.VerificationtRequests.Add(request);
        }

        public async Task AddVerificationRequestAsync(VerificationtRequestDto request)
        {
            if (request == null)
                return;

            await _context.VerificationtRequests.AddAsync(request);
        }

        public void RemoveVerificationRequest(int id)
        {
            var request = this.FindVerificationtRequest(id);
            if (request == null)
                return;

            _context.VerificationtRequests.Remove(request);
        }

        public void RemoveVerificationRequestByPhone(string number)
        {
            var request = this.FindVerificationtRequestByPhone(number);
            if (request == null)
                return;

            _context.VerificationtRequests.Remove(request);
        }
    }
}
