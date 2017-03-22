using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YoApp.Data.Models;

namespace YoApp.Data.Repositories
{
    public class VerificationTokensRepository : IVerificationTokensRepository
    {
        private readonly ApplicationDbContext _context;

        public VerificationTokensRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public VerificationToken FindById(int id)
        {
            return _context
                .VerificationTokens
                .SingleOrDefault(vr => vr.Id == id);
        }

        public async Task<VerificationToken> FindByIdAsync(int id)
        {
            return await _context
                .VerificationTokens
                .SingleOrDefaultAsync(vr => vr.Id == id);
        }

        public VerificationToken FindByUser(string number)
        {
            return _context
                .VerificationTokens
                .SingleOrDefault(vr => vr.User == number);
        }

        public async Task<VerificationToken> FindByUserAsync(string number)
        {
            return await _context
                .VerificationTokens
                .SingleOrDefaultAsync(vr => vr.User == number);
        }

        public VerificationToken FindByCode(string code)
        {
            return _context
                .VerificationTokens
                .SingleOrDefault(vr => vr.Code == code);
        }

        public async Task<VerificationToken> FindByCodeAsync(string code)
        {
            return await _context
                .VerificationTokens
                .SingleOrDefaultAsync(vr => vr.Code == code);
        }

        public void Add(VerificationToken request)
        {
            if(request == null)
                return;

            _context.VerificationTokens.Add(request);
        }

        public async Task AddAsync(VerificationToken request)
        {
            if (request == null)
                return;

            await _context.VerificationTokens.AddAsync(request);
        }

        public void AddOrReplace(VerificationToken request)
        {
            RemoveByUser(request.User);
            Add(request);
        }

        public async Task AddOrReplaceAsync(VerificationToken request)
        {
            RemoveByUser(request.User);
            await AddAsync(request);
        }

        public void RemoveById(int id)
        {
            var request = this.FindById(id);
            if (request == null)
                return;

            _context.VerificationTokens.Remove(request);
        }

        public void RemoveByUser(string number)
        {
            var request = this.FindByUser(number);
            if (request == null)
                return;

            _context.VerificationTokens.Remove(request);
        }
    }
}
