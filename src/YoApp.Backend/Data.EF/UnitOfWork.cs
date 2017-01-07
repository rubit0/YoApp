using System.Threading.Tasks;
using YoApp.Backend.Data.Repositories;

namespace YoApp.Backend.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository UserRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            UserRepository = userRepository;
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
