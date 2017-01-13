using System.Threading.Tasks;
using YoApp.Backend.Data.Repositories;

namespace YoApp.Backend.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository UserRepository { get; }
        public IVerificationRequestsRepository VerificationRequestsRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IVerificationRequestsRepository verificationRequestsRepository)
        {
            _context = context;
            UserRepository = userRepository;
            VerificationRequestsRepository = verificationRequestsRepository;
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
