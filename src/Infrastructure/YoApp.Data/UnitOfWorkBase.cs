using System.Threading.Tasks;

namespace YoApp.Data
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWorkBase(ApplicationDbContext context)
        {
            _context = context;
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
