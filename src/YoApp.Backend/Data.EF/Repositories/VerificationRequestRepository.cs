using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YoApp.Backend.Data.Repositories;
using YoApp.Backend.Models;

namespace YoApp.Backend.Data.EF.Repositories
{
    public class VerificationRequestRepository : IVerificationRequestsRepository
    {
        private readonly ApplicationDbContext _context;

        public VerificationRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public VerificationtRequest FindVerificationtRequest(int id)
        {
            return _context
                .VerificationtRequests
                .SingleOrDefault(vr => vr.Id == id);
        }

        public async Task<VerificationtRequest> FindVerificationtRequestAsync(int id)
        {
            return await _context
                .VerificationtRequests
                .SingleOrDefaultAsync(vr => vr.Id == id);
        }

        public VerificationtRequest FindVerificationtRequestByPhone(string number)
        {
            return _context
                .VerificationtRequests
                .SingleOrDefault(vr => vr.PhoneNumber == number);
        }

        public async Task<VerificationtRequest> FindVerificationtRequestByPhoneAsync(string number)
        {
            return await _context
                .VerificationtRequests
                .SingleOrDefaultAsync(vr => vr.PhoneNumber == number);
        }

        public VerificationtRequest FindVerificationtRequestByCode(string code)
        {
            return _context
                .VerificationtRequests
                .SingleOrDefault(vr => vr.VerificationCode == code);
        }

        public async Task<VerificationtRequest> FindVerificationtRequestByCodeAsync(string code)
        {
            return await _context
                .VerificationtRequests
                .SingleOrDefaultAsync(vr => vr.VerificationCode == code);
        }

        public void AddVerificationRequest(VerificationtRequest request)
        {
            if(request == null)
                return;

            _context.VerificationtRequests.Add(request);
        }

        public async Task AddVerificationRequestAsync(VerificationtRequest request)
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
