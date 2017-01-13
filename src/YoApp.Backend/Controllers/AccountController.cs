using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;
using YoApp.Backend.DataObjects.Account;
using YoApp.Backend.Models;
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

        [HttpPost("StartVerification")]
        public async Task<IActionResult> StartVerification([FromForm]InitialVerificationForm form)
        {
            if (!ModelState.IsValid || !form.IsModelValid())
                return BadRequest();

            if (!form.CheckIsValidCountryCode())
                return BadRequest($"Country [{form.CountryCode}] is not supported.");

            var number = form.GetFormatedPhoneNumber();
            _logger.LogInformation($"The PhoneNumber [{number}] is requesting an verification code");

            var request = VerificationtRequest.CreateVerificationtRequest(number);

            var sendingResult = await _messageSender.SendMessageAsync("+" + number, $"Hello from YoApp!\nYour verification Code is:\n{request.VerificationCode}");
            if(!sendingResult)
                return new StatusCodeResult(500);

            //remove potentially previous request
            _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequestByPhone(number);

            await _unitOfWork.VerificationRequestsRepository.AddVerificationRequestAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok(VerificationCode.CreateFromVerificationRequest(request));
        }
    }
}
