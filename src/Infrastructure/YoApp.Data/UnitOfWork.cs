using System.Threading.Tasks;
using YoApp.Data.Repositories;

namespace YoApp.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IFriendsRepository UserRepository { get; }
        public IVerificationTokensRepository VerificationTokensRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IFriendsRepository userRepository, IVerificationTokensRepository verificationTokensRepository)
        {
            _context = context;
            UserRepository = userRepository;
            VerificationTokensRepository = verificationTokensRepository;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
