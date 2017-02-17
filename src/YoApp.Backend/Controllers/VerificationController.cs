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
        public async Task<IActionResult> ChallengeVerification([FromForm]VerificationChallengeDto challenge)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(!_configurationService.ValidCountryCallCodes.Contains(challenge.CountryCodeToInt()))
                return BadRequest($"Country [{challenge.CountryCode}] is not supported.");

            var number = challenge.ToString();
            _logger.LogInformation($"The PhoneNumber [{number}] is requesting an verification code.");

            //Generate request object
            var request = new VerificationtRequest(number, _configurationService.VerificationDuration, CodeGenerator.GetCode());

            //Send SMS message with code to client
            var clientMessage = $"Hello from YoApp!\nYour verification Code is:\n{request.VerificationCode}";
#if !DEBUG
            var sendingResult = await _messageSender.SendMessageAsync("+" + number, clientMessage);
            if (!sendingResult)
                return new StatusCodeResult(500);
#endif
#if DEBUG
            _logger.LogDebug($"Verification Code for {request.PhoneNumber} is: [{request.VerificationCode}]");
            _logger.LogInformation($"Message send to client:\n{clientMessage}");
#endif
            //Persist the request to resolve it later
            await _unitOfWork.VerificationRequestsRepository.AddOrReplaceAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("Resolve")]
        public async Task<IActionResult> ResolveVerification([FromForm]VerificationResolveDto resolve)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //Retrieve request from db
            var request = await _unitOfWork
                .VerificationRequestsRepository
                .FindByPhoneAsync(resolve.PhoneNumber);

            if (request == null)
                return BadRequest($"No verification request found for {resolve.PhoneNumber}.");

            //Verify if code matches
            if (!request.VerifyFromRequest(resolve))
            {
                _logger.LogInformation($"Code verification failed for [+{resolve.PhoneNumber}.\nExpected ({request.VerificationCode}) but got ({resolve.VerificationCode}).]");
                return BadRequest("Verification code does not match.");
            }

            //Check if the user already has an account, otherwise create and persist a new one
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(resolve.PhoneNumber);
            if (user == null)
            {
                user = new ApplicationUser { UserName = resolve.PhoneNumber, Nickname = string.Empty };
                var creationResult = await _unitOfWork.UserRepository.AddAsync(user, resolve.Password);
                if (!creationResult.Succeeded)
                    return StatusCode(500);

                _logger.LogInformation($"A new User have been created [{user.UserName}].");
            }
            else
            {
                await _unitOfWork.UserRepository.UpdatePasswordAsync(user, resolve.Password);
            }

            //At this step the user is verified and persistet, remove obsolet request from db
            _unitOfWork.VerificationRequestsRepository.RemoveById(request.Id);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Verification was succesfull for User [{user.UserName}.]");

            return Ok();
        }
    }
}
