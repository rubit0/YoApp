using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;

namespace YoApp.Backend.Controllers
{
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

        [Authorize]
        [HttpPost("UpdateNickName")]
        public async Task<IActionResult> UpdateNickName(string name)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
                return StatusCode(500);

            user.NickName = name;
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Updated NickName for User [{user.UserName}.]");

            return Ok();
        }

        [Authorize]
        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpateStatus(string status)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
                return StatusCode(500);

            user.Status = status ?? "";
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Updated status message for User [{user.UserName}.]");

            return Ok();
        }
    }
}
