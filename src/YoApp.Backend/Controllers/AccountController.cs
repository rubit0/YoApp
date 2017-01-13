using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork, IMessageSender messageSender, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _messageSender = messageSender;
            _userManager = userManager;
            _signInManager = signInManager;
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

            return Ok(VerificationChallenge.CreateFromVerificationRequest(request));
        }

        [HttpPost("ResolveVerification")]
        public async Task<IActionResult> ResolveVerification([FromForm]VerificationResponse response)
        {
            if (!ModelState.IsValid || !response.IsModelValid())
                return BadRequest();

            var request = await _unitOfWork
                .VerificationRequestsRepository
                .FindVerificationtRequestByPhoneAsync(response.PhoneNumber);

            if (request == null)
                return BadRequest("No verification request found.");

            if(!response.Verify(request))
                return BadRequest("Verification code does not match.");

            var user = await _userManager.FindByNameAsync(response.PhoneNumber);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = response.PhoneNumber,
                    PhoneNumber = response.PhoneNumber,
                    PhoneNumberConfirmed = true
                };

                var creationResult = await _userManager.CreateAsync(user, response.Password);
                if (!creationResult.Succeeded)
                    return StatusCode(500);

                _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequest(request.Id);
                await _unitOfWork.CompleteAsync();

                return Ok();
            }

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, response.Password);

            _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequest(request.Id);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
