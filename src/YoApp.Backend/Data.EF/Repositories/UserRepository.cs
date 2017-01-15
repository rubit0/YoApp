using System;
using System.Collections.Generic;
using System.Linq;
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

        public ApplicationUser GetUserFromPhoneNumber(string phoneNumber)
        {
            return this.GetUserFromPhoneNumberAsync(phoneNumber).Result;
        }

        public async Task<ApplicationUser> GetUserFromPhoneNumberAsync(string phoneNumber)
        {
            return await _userManager.FindByNameAsync(phoneNumber);
        }

        public IEnumerable<ApplicationUser> GetUsersFromPhoneNumbers(IEnumerable<string> phoneNumbers)
        {
            return this.GetUsersFromPhoneNumbersAsync(phoneNumbers).Result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersFromPhoneNumbersAsync(IEnumerable<string> phoneNumbers)
        {
            List<ApplicationUser> usersInDb = new List<ApplicationUser>();

            foreach (var phoneNumber in phoneNumbers)
            {
                var userInDb = await _userManager.FindByNameAsync(phoneNumber);
                if (userInDb != null)
                    usersInDb.Add(userInDb);
            }

            return usersInDb;
        }
    }
}
