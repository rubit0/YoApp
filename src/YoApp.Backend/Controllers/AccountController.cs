using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;

namespace YoApp.Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Nickname")]
        public async Task<IActionResult> GetNickname()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            return Ok(user.Nickname);
        }

        [HttpPut("Nickname")]
        public async Task<IActionResult> UpdateNickname([FromForm]string name)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            user.Nickname = name;
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Updated Nickname for User [{user.Nickname}.]");

            return Ok(user.Nickname);
        }

        [HttpGet("Status")]
        public async Task<IActionResult> GetStatus()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            return Ok(user.Status);
        }

        [HttpPut("Status")]
        public async Task<IActionResult> UpateStatus([FromForm]string status)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            user.Status = status;
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Updated status message for User [{user.Status}.]");

            return Ok(user.Status);
        }
    }
}
