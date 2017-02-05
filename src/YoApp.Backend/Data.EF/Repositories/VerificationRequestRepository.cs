using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YoApp.Backend.Data.Repositories;
using YoApp.Backend.Models;
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

        public VerificationtRequest FindById(int id)
        {
            return _context
                .VerificationRequests
                .SingleOrDefault(vr => vr.Id == id);
        }

        public async Task<VerificationtRequest> FindByIdAsync(int id)
        {
            return await _context
                .VerificationRequests
                .SingleOrDefaultAsync(vr => vr.Id == id);
        }

        public VerificationtRequest FindByPhone(string number)
        {
            return _context
                .VerificationRequests
                .SingleOrDefault(vr => vr.PhoneNumber == number);
        }

        public async Task<VerificationtRequest> FindByPhoneAsync(string number)
        {
            return await _context
                .VerificationRequests
                .SingleOrDefaultAsync(vr => vr.PhoneNumber == number);
        }

        public VerificationtRequest FindByCode(string code)
        {
            return _context
                .VerificationRequests
                .SingleOrDefault(vr => vr.VerificationCode == code);
        }

        public async Task<VerificationtRequest> FindByCodeAsync(string code)
        {
            return await _context
                .VerificationRequests
                .SingleOrDefaultAsync(vr => vr.VerificationCode == code);
        }

        public void Add(VerificationtRequest request)
        {
            if(request == null)
                return;

            _context.VerificationRequests.Add(request);
        }

        public async Task AddAsync(VerificationtRequest request)
        {
            if (request == null)
                return;

            await _context.VerificationRequests.AddAsync(request);
        }

        public void AddOrReplace(VerificationtRequest request)
        {
            RemoveById(request.Id);
            Add(request);
        }

        public async Task AddOrReplaceAsync(VerificationtRequest request)
        {
            RemoveById(request.Id);
            await AddAsync(request);
        }

        public void RemoveById(int id)
        {
            var request = this.FindById(id);
            if (request == null)
                return;

            _context.VerificationRequests.Remove(request);
        }

        public void RemoveByPhone(string number)
        {
            var request = this.FindByPhone(number);
            if (request == null)
                return;

            _context.VerificationRequests.Remove(request);
        }
    }
}
