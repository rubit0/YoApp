using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using YoApp.Core.Dtos.Account;
using YoApp.Core.Models;

namespace YoApp.Identity.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var userInDb = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userInDb == null)
                return StatusCode(500);

            var dto = new UpdatedAccountDto
            {
                Nickname = userInDb.Nickname,
                StatusMessage = userInDb.Status
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdatedAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userInDb = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userInDb == null)
                return NotFound();

            if (string.CompareOrdinal(userInDb.Nickname, dto.Nickname) != 0)
            {
                userInDb.Nickname = dto.Nickname;
                _logger.LogInformation($"{userInDb.Nickname} changed Nickname to: {dto.Nickname}");
            }

            if (string.CompareOrdinal(userInDb.Status, dto.StatusMessage) != 0)
            {
                userInDb.Status = dto.StatusMessage;
                _logger.LogInformation($"{userInDb.Status} changed Status to: {dto.StatusMessage}");
            }

            var result = await _userManager.UpdateAsync(userInDb);
            if (!result.Succeeded)
                return StatusCode(500);

            return Ok();
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetName()
        {
            var userInDb = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userInDb == null)
                return StatusCode(500);

            return Ok(userInDb.Nickname);
        }

        [HttpPatch("name/{name}")]
        public async Task<IActionResult> UpdateName(string name)
        {
            var userInDb = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userInDb == null)
                return StatusCode(500);

            userInDb.Nickname = name;
            var result = await _userManager.UpdateAsync(userInDb);
            if (!result.Succeeded)
                return StatusCode(500);

            _logger.LogInformation($"Updated nickname for {userInDb.Nickname} to: {userInDb.Nickname}");
            return Ok();
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var userInDb = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userInDb == null)
                return StatusCode(500);

            return Ok(userInDb.Status);
        }

        [HttpPatch("status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status)
        {
            var userInDb = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userInDb == null)
                return StatusCode(500);

            userInDb.Status = status;
            var result = await _userManager.UpdateAsync(userInDb);
            if (!result.Succeeded)
                return StatusCode(500);

            _logger.LogInformation($"Updated {userInDb.UserName} status to: {userInDb.Status}");
            return Ok();
        }
    }
}
