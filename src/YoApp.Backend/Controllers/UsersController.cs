using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YoApp.Backend.Data;
using YoApp.DataObjects.Users;

namespace YoApp.Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var userInDb = await _unitOfWork.UserRepository.GetUserAsync(phoneNumber);
            if (userInDb == null)
                return BadRequest();

            return Ok(userInDb);
        }

        //Using POST Verb due to long querry object
        [HttpPost]
        public async Task<IActionResult> GetUsers(IEnumerable<string> phoneNumbers)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usersInDb = await _unitOfWork.UserRepository.GetUsersAsync(phoneNumbers);
            if (!usersInDb.Any())
                return NoContent();

            var matches = _mapper.Map<IEnumerable<UsersDto>>(usersInDb);

            return Ok(matches);
        }
    }
}
