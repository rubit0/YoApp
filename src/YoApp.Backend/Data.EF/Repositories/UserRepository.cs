using System.Linq;
using YoApp.Backend.Data.Repositories;

namespace YoApp.Backend.Data.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsPhoneNumberTaken(string phoneNumber)
        {
            return _context.Users.Any(u => u.UserName == phoneNumber);
        }
    }
}
