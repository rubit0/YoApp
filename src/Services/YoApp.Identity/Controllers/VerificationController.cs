using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Identity.Helper;
using YoApp.Identity.Services.Interfaces;
using YoApp.DataObjects.Verification;
using YoApp.Utils.Misc;
using YoApp.Data;
using YoApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace YoApp.Identity.Controllers
{
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsSender _messageSender;
        private readonly IConfigurationService _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerificationController(ILogger<VerificationController> logger, IUnitOfWork unitOfWork, ISmsSender messageSender, IConfigurationService configuration, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _messageSender = messageSender;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("challenge")]
        public async Task<IActionResult> ChallengeVerification([FromForm]VerificationChallengeDto challenge)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(_configuration.CountriesBlackList.Contains(challenge.CountryCodeToInt()))
                return BadRequest($"Country [{challenge.CountryCode}] is not supported.");

            var number = challenge.ToString();
            _logger.LogInformation($"The PhoneNumber [{number}] is requesting an verification code.");

            //Create request object
            var request = new VerificationToken(number, _configuration.VerificationDuration, CodeGenerator.GetCode());

            //Send SMS message with code to client
            var clientMessage = $"Hello from YoApp!\nYour verification Code is:\n{request.Code}";

            var sendingResult = await _messageSender.SendMessageAsync("+" + number, clientMessage);
            if (!sendingResult)
                return new StatusCodeResult(500);

            //Persist the request to resolve it later
            await _unitOfWork.VerificationTokensRepository.AddOrReplaceAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("resolve")]
        public async Task<IActionResult> ResolveVerification([FromForm]VerificationResolveDto resolve)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //Retrieve request from db
            var request = await _unitOfWork
                .VerificationTokensRepository
                .FindByUserAsync(resolve.PhoneNumber);

            if (request == null)
                return BadRequest($"No verification request found for {resolve.PhoneNumber}.");

            //Verify if code matches
            if (!request.ResolveToken(resolve))
            {
                _logger.LogInformation($"Code verification failed for [+{resolve.PhoneNumber}.\nExpected ({request.Code}) but got ({resolve.VerificationCode}).]");
                return BadRequest("Verification code does not match.");
            }

            if (request.IsExpired())
            {
                _logger.LogWarning($"Token has expired, challenge again.");
                _unitOfWork.VerificationTokensRepository.RemoveById(request.Id);
                await _unitOfWork.CompleteAsync();

                return BadRequest("Token expired");
            }

            //Check if the user already has an account, otherwise create and persist a new one
            var user = await _userManager.FindByNameAsync(resolve.PhoneNumber);
            if (user == null)
            {
                user = new ApplicationUser { UserName = resolve.PhoneNumber, Nickname = string.Empty };

                var creationResult = await _userManager.CreateAsync(user, resolve.Password);
                if (!creationResult.Succeeded)
                    return StatusCode(500);

                _logger.LogInformation($"A new User have been created [{user.UserName}].");
            }
            else
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, resolve.Password);
            }

            //At this step the user is verified and persistet, remove obsolet request from db
            _unitOfWork.VerificationTokensRepository.RemoveById(request.Id);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Verification was succesfull for User [{user.UserName}.]");

            return Ok();
        }
    }
}
