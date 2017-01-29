using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YoApp.Backend.Data;
using YoApp.Backend.DataObjects.Users;

namespace YoApp.Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string phoneNumber)
        {
            var userInDb = await _unitOfWork.UserRepository.GetUserAsync(phoneNumber);
            if (userInDb == null)
                return BadRequest();

            return Ok(userInDb);
        }

        //Using POST Verb due to long querry object
        [HttpPost]
        public async Task<IActionResult> GetUsers(IEnumerable<string> phoneNumbers)
        {
            var usersInDb = await _unitOfWork.UserRepository.GetUsersAsync(phoneNumbers);
            if (!usersInDb.Any())
                return NoContent();

            var matches = UsersDto.MapFromAppUsers(usersInDb);

            return Ok(matches);
        }
    }
}
