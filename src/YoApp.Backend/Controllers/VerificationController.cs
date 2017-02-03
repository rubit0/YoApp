using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Backend.Data;
using YoApp.Backend.Helper;
using YoApp.Backend.Models;
using YoApp.Backend.Services.Interfaces;
using YoApp.DataObjects.Verification;

namespace YoApp.Backend.Controllers
{
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsSender _messageSender;
        private readonly IConfigurationService _configurationService;

        public VerificationController(ILogger<VerificationController> logger, IUnitOfWork unitOfWork, ISmsSender messageSender, IConfigurationService configurationService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _messageSender = messageSender;
            _configurationService = configurationService;
        }

        [HttpPost("Challenge")]
        public async Task<IActionResult> ChallengeVerification([FromForm]VerificationFormDto form)
        {
            if (!ModelState.IsValid || !form.IsModelValid())
                return BadRequest();

            if(!_configurationService.ValidCountryCallCodes.Contains(form.CountryCode))
                return BadRequest($"Country [{form.CountryCode}] is not supported.");

            var number = form.GetFormatedPhoneNumber();
            _logger.LogInformation($"The PhoneNumber [{number}] is requesting an verification code.");

            //Generate request object
            var request = new VerificationtRequestDto(number, _configurationService.VerificationDuration, GenerateVerificationCode());
            var clientMessage = $"Hello from YoApp!\nYour verification Code is:\n{request.VerificationCode}";

            //Send SMS message with code to client
            var sendingResult = await _messageSender.SendMessageAsync("+" + number, clientMessage);
            if (!sendingResult)
                return new StatusCodeResult(500);

#if DEBUG
            _logger.LogDebug($"Verification Code for {request.PhoneNumber} is: [{request.VerificationCode}]");
            _logger.LogInformation($"Message send to client:\n{clientMessage}");
#endif

            //remove potentially previous request
            _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequestByPhone(number);

            //Persist the request to resolve it later
            await _unitOfWork.VerificationRequestsRepository.AddVerificationRequestAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("Resolve")]
        public async Task<IActionResult> ResolveVerification([FromForm]VerificationResponseDto response)
        {
            if (!ModelState.IsValid || !response.IsModelValid())
                return BadRequest();

            //Retrieve request from db
            var request = await _unitOfWork
                .VerificationRequestsRepository
                .FindVerificationtRequestByPhoneAsync(response.PhoneNumber);

            if (request == null)
                return BadRequest($"No verification request found for {response.PhoneNumber}.");

            //Verify if code matches
            if (!response.VerifyFromRequest(request))
            {
                _logger.LogInformation($"Code verification failed for [+{response.PhoneNumber}.\nExpected ({request.VerificationCode}) but got ({response.VerificationCode}).]");
                return BadRequest("Verification code does not match.");
            }

            //Check if the user already has an account, otherwise create and persist a new one
            var user = await _unitOfWork.UserRepository.GetUserAsync(response.PhoneNumber);
            if (user == null)
            {
                user = new ApplicationUser { UserName = response.PhoneNumber, Nickname = response.PhoneNumber };
                var creationResult = await _unitOfWork.UserRepository.AddUserAsync(user, response.Password);
                if (!creationResult.Succeeded)
                    return StatusCode(500);

                _logger.LogInformation($"A new User have been created [{user.UserName}].");
            }
            else
            {
                await _unitOfWork.UserRepository.UpdateUserPasswordAsync(user, response.Password);
            }

            //At this step the user is verified and persistet, remove obsolet request from db
            _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequest(request.Id);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Verification was succesfull for User [{user.UserName}.]");

            return Ok();
        }

        private static readonly Random RandomGenerator = new Random();

        public static string GenerateVerificationCode()
        {
            return $"{RandomGenerator.Next(100, 1000)}-{RandomGenerator.Next(100, 1000)}";
        }
    }
}
