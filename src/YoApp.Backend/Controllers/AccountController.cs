using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;
using YoApp.DataObjects.Account;

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

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdatedAccountDto dto)
        {
            var userInDb = await _unitOfWork.UserRepository.GetByUsernameAsync(User.Identity.Name);
            if (userInDb == null)
                return NotFound();

            if (string.CompareOrdinal(userInDb.Nickname, dto.Nickname) != 0)
                userInDb.Nickname = dto.Nickname;

            if (string.CompareOrdinal(userInDb.Status, dto.StatusMessage) != 0)
                userInDb.Status = dto.StatusMessage;

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var userInDb = await _unitOfWork.UserRepository.GetByUsernameAsync(User.Identity.Name);
            if (userInDb == null)
                return StatusCode(500);

            var dto = new UpdatedAccountDto
            {
                Nickname = userInDb.Nickname,
                StatusMessage = userInDb.Status
            };

            return Ok(dto);
        }
    }
}
