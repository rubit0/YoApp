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

        public async Task<VerificationToken> FindByIdAsync(int id)
        {
            return await _context
                .VerificationTokens
                .SingleOrDefaultAsync(vr => vr.Id == id);
        }

        public async Task<VerificationToken> FindByUserAsync(string number)
        {
            return await _context
                .VerificationTokens
                .SingleOrDefaultAsync(vr => vr.User == number);
        }

        public async Task<VerificationToken> FindByCodeAsync(string code)
        {
            return await _context
                .VerificationTokens
                .SingleOrDefaultAsync(vr => vr.Code == code);
        }

        public async Task AddAsync(VerificationToken request)
        {
            if (request == null)
                return;

            await _context.VerificationTokens.AddAsync(request);
        }

        public async Task AddOrReplaceAsync(VerificationToken request)
        {
            var requestInDb = await FindByIdAsync(request.Id);
            if(requestInDb != null)
                Remove(requestInDb);

            await AddAsync(request);
        }

        public void Remove(VerificationToken token)
        {
            _context.VerificationTokens.Remove(token);
        }
    }
}
