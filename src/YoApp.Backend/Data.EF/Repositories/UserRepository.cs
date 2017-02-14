using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Backend.Data.Repositories;
using YoApp.Backend.Models;

namespace YoApp.Backend.Data.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser GetByUsername(string name)
        {
            return this.GetByUsernameAsync(name).Result;
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public IEnumerable<ApplicationUser> GetByUsernames(IEnumerable<string> names)
        {
            return this.GetByUsernamesAsync(names).Result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetByUsernamesAsync(IEnumerable<string> names)
        {
            var usersInDb = new List<ApplicationUser>();

            foreach (var phoneNumber in names)
            {
                var userInDb = await _userManager.FindByNameAsync(phoneNumber);
                if (userInDb != null)
                    usersInDb.Add(userInDb);
            }

            return usersInDb;
        }

        public bool IsMember(string name)
        {
            return _userManager.FindByNameAsync(name) != null;
        }

        public IdentityResult Add(ApplicationUser user, string password)
        {
            return AddAsync(user, password).Result;
        }

        public async Task<IdentityResult> AddAsync(ApplicationUser user, string password)
        {
            if (user == null || string.IsNullOrWhiteSpace(password))
                return IdentityResult.Failed();

            return await _userManager.CreateAsync(user, password);
        }

        public void UpdatePassword(ApplicationUser user, string password)
        {
            UpdatePasswordAsync(user, password).RunSynchronously();
        }

        public async Task UpdatePasswordAsync(ApplicationUser user, string password)
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, password);
        }
    }
}
