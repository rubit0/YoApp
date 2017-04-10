using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Core.Models;

namespace YoApp.Data.Repositories
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendsRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<IEnumerable<ApplicationUser>> FindByNameRangeAsync(IEnumerable<string> names)
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

        public async Task<IEnumerable<string>> IsMemberRangeAsync(IEnumerable<string> names)
        {
            var users = new List<string>();

            foreach (var name in names)
            {
                var user = await _userManager.FindByNameAsync(name);
                if (user != null)
                    users.Add(user.UserName);
            }

            return users;
        }
    }
}
