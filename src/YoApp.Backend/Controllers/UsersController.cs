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

        [HttpPost("Match")]
        public async Task<IActionResult> Match(IEnumerable<UsersDto> contacts)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usersInDb = await _unitOfWork
                .UserRepository
                .GetUsersAsync(contacts.Select(c => c.PhoneNumber));

            if (!usersInDb.Any())
                return NoContent();

            var matches = UsersDto.MapFromAppUsers(usersInDb);
            return Ok(matches);
        }

        [HttpGet("GetContact")]
        public async Task<IActionResult> GetContact(string phoneNumber)
        {
            var userInDb = await _unitOfWork.UserRepository.GetUserAsync(phoneNumber);
            if (userInDb == null)
                return BadRequest();

            return Ok(userInDb);
        }
    }
}
