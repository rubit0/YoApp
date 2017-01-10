using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;
using YoApp.Backend.DataObjects.Account;
using YoApp.Backend.Services.Interfaces;

namespace YoApp.Backend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageSender _messageSender;

        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork, IMessageSender messageSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _messageSender = messageSender;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }

        [HttpPost("InitialSetup")]
        public async Task<IActionResult> InitialSetup(InitialUserCreationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_unitOfWork.UserRepository.IsPhoneNumberTaken(form.GetValidPhoneNumber()))
                return BadRequest("Phonenumber already taken");

            _logger.LogInformation($"The PhoneNumber [{form.GetValidPhoneNumber()}] is requesting an setup code");

            await _messageSender.SendMessageAsync(form.GetValidPhoneNumber(), "Hello from YoApp!");

            return Ok();
        }
    }
}
