using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoApp.Identity.Helper;
using YoApp.Identity.Services.Interfaces;
using YoApp.Utils.Misc;
using Microsoft.AspNetCore.Identity;
using YoApp.Core.Dtos.Verification;
using YoApp.Core.Models;

namespace YoApp.Identity.Controllers
{
    [Route("[controller]")]
    public class VerificationController : Controller
    {
        private readonly ILogger _logger;
        private readonly IIdentityPersistence _dataWorker;
        private readonly ISmsSender _messageSender;
        private readonly IConfigurationService _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerificationController(ILogger<VerificationController> logger, IIdentityPersistence dataWorker, ISmsSender messageSender, 
            IConfigurationService configuration, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _dataWorker = dataWorker;
            _messageSender = messageSender;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestVerification([FromForm]VerificationChallengeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(_configuration.CountriesBlackList.Contains(dto.ParseCountryCode()))
                return BadRequest($"Country [{dto.CountryCode}] is not supported.");

            var number = dto.ToString();
            _logger.LogInformation($"The PhoneNumber [{number}] is requesting an verification code.");

            //Create request object
            var request = new VerificationToken(number, _configuration.VerificationDuration, CodeGenerator.GetCode());

            //Send SMS message with code to client
            var clientMessage = $"Hello from YoApp!\nYour verification Code is:\n{request.Code}";

            var sendingResult = await _messageSender.SendMessageAsync("+" + number, clientMessage);
            if (!sendingResult)
                return new StatusCodeResult(500);

            //Persist the request to resolve it later
            await _dataWorker.VerificationTokens.AddOrReplaceAsync(request);
            await _dataWorker.CompleteAsync();

            return Ok();
        }

        [HttpPost("resolve")]
        public async Task<IActionResult> ResolveVerification([FromForm]VerificationResolveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //Retrieve request from db
            var request = await _dataWorker
                .VerificationTokens
                .FindByUserAsync(dto.PhoneNumber);

            if (request == null)
                return BadRequest($"No verification request found for {dto.PhoneNumber}.");

            //Verify if code matches
            if (!request.ResolveToken(dto))
            {
                _logger.LogInformation($"Code verification failed for [+{dto.PhoneNumber}.\nExpected ({request.Code}) but got ({dto.VerificationCode}).]");
                return BadRequest("Verification code does not match.");
            }

            if (request.IsExpired())
            {
                _logger.LogWarning($"Token has expired, challenge again.");
                _dataWorker.VerificationTokens.Remove(request);
                await _dataWorker.CompleteAsync();

                return BadRequest("Token expired");
            }

            //Check if the user already has an account, otherwise create and persist a new one
            var user = await _userManager.FindByNameAsync(dto.PhoneNumber);
            if (user == null)
            {
                user = new ApplicationUser { UserName = dto.PhoneNumber, Nickname = string.Empty };

                var creationResult = await _userManager.CreateAsync(user, dto.Password);
                if (!creationResult.Succeeded)
                    return StatusCode(500);

                _logger.LogInformation($"A new User have been created [{user.UserName}].");
            }
            else
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, dto.Password);
            }

            //At this step the user is verified and persistet, remove obsolet request from db
            _dataWorker.VerificationTokens.Remove(request);
            await _dataWorker.CompleteAsync();
            _logger.LogInformation($"Verification was succesfull for User [{user.UserName}.]");

            return Ok();
        }
    }
}
