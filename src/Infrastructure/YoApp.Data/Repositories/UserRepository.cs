using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Data.Models;

namespace YoApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser GetByName(string name)
        {
            return this.GetByNameAsync(name).Result;
        }

        public async Task<ApplicationUser> GetByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public IEnumerable<ApplicationUser> GetByNames(IEnumerable<string> names)
        {
            return this.GetByNamesAsync(names).Result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetByNamesAsync(IEnumerable<string> names)
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

        public async Task<bool> IsMemberAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user != null;
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
