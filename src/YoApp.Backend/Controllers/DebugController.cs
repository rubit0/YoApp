using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;
using YoApp.Backend.Models;

namespace YoApp.Backend.Controllers
{
#if DEBUG
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class DebugController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DebugController(ILogger<DebugController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStub([FromForm]string phone, string nickname, string status, string password)
        {
            phone = Regex.Replace(phone, @"\D", string.Empty);
            if (nickname.Length > 20 || status.Length > 30)
                return BadRequest("Bad Model");


            var userInDb = await _unitOfWork.UserRepository.IsMemberAsync(phone);
            if (userInDb)
                return BadRequest("User already in db");

            var user = new ApplicationUser { UserName = phone, Nickname = nickname, Status = status};

            var creationResult = await _unitOfWork.UserRepository.AddAsync(user, password);
            if (!creationResult.Succeeded)
            {
                _logger.LogError($"Couldn't create an User for this request with number: {phone}");
                return StatusCode(500);
            }

            _logger.LogInformation($"Created an User with number: {phone}");
            return Ok();
        }
    } 
#endif
}
