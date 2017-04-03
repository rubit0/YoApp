using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.DataObjects.Users;
using YoApp.Data.Models;
using YoApp.Friends.Helper;

namespace YoApp.Identity.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FriendsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFriendsPersistence _dataWorker;
        private readonly IMapper _mapper;

        public FriendsController(ILogger<FriendsController> logger, IFriendsPersistence dataWorker, IMapper mapper)
        {
            _logger = logger;
            _dataWorker = dataWorker;
            _mapper = mapper;
        }

        [HttpGet("{phoneNumber}")]
        public async Task<IActionResult> FindUser(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var userInDb = await _dataWorker.Friends.FindByNameAsync(phoneNumber);
            if (userInDb == null)
            {
                _logger.LogError($"Request by [{User.Identity.Name}].\nNo User found by phone number [{phoneNumber}].");
                return NotFound();
            }

            var dto = _mapper.Map<UserDto>(userInDb);
            
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> FindUsers([FromBody]IEnumerable<string> phoneNumbers)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usersInDb = await _dataWorker.Friends.FindByNameRangeAsync(phoneNumbers);
            if (!usersInDb.Any())
            {
                _logger.LogError($"Request by [{User.Identity.Name}].\nNo matching Users found.");
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

            var userInDb = await _dataWorker.Friends.FindByNameAsync(phoneNumber);
            if (userInDb == null)
            {
                _logger.LogError($"Request by [{User.Identity.Name}].\nUser name for {phoneNumber} was not found.");
                return NotFound();
            }

            return Ok(userInDb.Nickname);
        }

        [HttpGet("{phoneNumber}/status")]
        public async Task<IActionResult> GetStatus(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var userInDb = await _dataWorker.Friends.FindByNameAsync(phoneNumber);
            if (userInDb == null)
            {
                _logger.LogError($"Request by [{User.Identity.Name}].\nStatus for {phoneNumber} was not found.");
                return NotFound();
            }

            return Ok(userInDb.Status);
        }

        [HttpGet("check/{phoneNumber}")]
        public async Task<IActionResult> IsMember(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest();

            var result = await _dataWorker.Friends.IsMemberAsync(phoneNumber);
            _logger.LogInformation($"Request by [{User.Identity.Name}].\nPhonenumber {phoneNumber} is member: {result}.");

            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpPost("check/")]
        public async Task<IActionResult> IsMemberRange([FromBody]IEnumerable<string> phoneNumbers)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usersInDb = await _dataWorker.Friends.FindByNameRangeAsync(phoneNumbers);
            var dto = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDto>>(usersInDb);

            return Ok(dto);
        }
    }
}
