using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.DataObjects.Users;
using YoApp.Data;

namespace YoApp.Identity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FriendsController(ILogger<FriendsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{phoneNumber}")]
        public async Task<IActionResult> FindUser(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var userInDb = await _unitOfWork.UserRepository.GetByNameAsync(phoneNumber);
            if (userInDb == null)
            {
                _logger.LogError($"User by {phoneNumber} requested by [{User.Identity.Name}] was not found.");
                return NotFound();
            }

            var dto = _mapper.Map<UserDto>(userInDb);
            
            return Ok(dto);
        }

        [HttpPost("{phoneNumbers}")]
        public async Task<IActionResult> FindUsers([FromBody]IEnumerable<string> phoneNumbers)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usersInDb = await _unitOfWork.UserRepository.GetByNamesAsync(phoneNumbers);
            if (!usersInDb.Any())
            {
                _logger.LogError($"No matching Users found for [{User.Identity.Name}] request.");
                return NotFound();
            }

            var matches = _mapper.Map<IEnumerable<UserDto>>(usersInDb);

            return Ok(matches);
        }

        [HttpGet("{phoneNumber}/name")]
        public async Task<IActionResult> GetName(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var userInDb = await _unitOfWork.UserRepository.GetByNameAsync(phoneNumber);
            if (userInDb == null)
            {
                _logger.LogError($"User by {phoneNumber} requested by [{User.Identity.Name}] was not found.");
                return NotFound();
            }

            return Ok(userInDb.Nickname);
        }

        [HttpGet("{phoneNumber}/status")]
        public async Task<IActionResult> GetStatus(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var userInDb = await _unitOfWork.UserRepository.GetByNameAsync(phoneNumber);
            if (userInDb == null)
            {
                _logger.LogError($"User by {phoneNumber} requested by [{User.Identity.Name}] was not found.");
                return NotFound();
            }

            return Ok(userInDb.Status);
        }

        [HttpGet("check/{phoneNumber}")]
        public async Task<IActionResult> IsMember(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var result = await _unitOfWork.UserRepository.IsMemberAsync(phoneNumber);

            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpPost("check/{phoneNumbers}")]
        public async Task<IActionResult> AreMember([FromBody]IEnumerable<string> phoneNumbers)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usersInDb = await _unitOfWork.UserRepository.GetByNamesAsync(phoneNumbers);

            return Ok(usersInDb);
        }
    }
}
