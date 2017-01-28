using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> StartVerification([FromForm]VerificationFormDto form)
        {
            if (!ModelState.IsValid || !form.IsModelValid())
                return BadRequest();

            if (!form.CheckIsValidCountryCode())
                return BadRequest($"Country [{form.CountryCode}] is not supported.");

            var number = form.GetFormatedPhoneNumber();
            _logger.LogInformation($"The PhoneNumber [{number}] is requesting an verification code.");

            var request = VerificationtRequest.CreateVerificationtRequest(number);
            var clientMessage = $"Hello from YoApp!\nYour verification Code is:\n{request.VerificationCode}";

            var sendingResult = await _messageSender.SendMessageAsync("+" + number, clientMessage);
            if(!sendingResult)
                return new StatusCodeResult(500);

#if DEBUG
            _logger.LogDebug($"Verification Code for {request.PhoneNumber} is: [{request.VerificationCode}]");
            _logger.LogInformation($"Message send to client:\n{clientMessage}");
#endif

            //remove potentially previous request
            _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequestByPhone(number);

            await _unitOfWork.VerificationRequestsRepository.AddVerificationRequestAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("ResolveVerification")]
        public async Task<IActionResult> ResolveVerification([FromForm]VerificationResponseDto response)
        {
            if (!ModelState.IsValid || !response.IsModelValid())
                return BadRequest();

            var request = await _unitOfWork
                .VerificationRequestsRepository
                .FindVerificationtRequestByPhoneAsync(response.PhoneNumber);

            if (request == null)
                return BadRequest($"No verification request found for {response.PhoneNumber}.");

            if (!response.Verify(request))
            {
                _logger.LogInformation($"Code verification failed for [+{response.PhoneNumber}.\nExpected ({request.VerificationCode}) but got ({response.VerificationCode}).]");
                return BadRequest("Verification code does not match.");
            }

            var user = await _unitOfWork.UserRepository.GetUserAsync(response.PhoneNumber);
            if (user == null)
            {
                user = new ApplicationUser { UserName = response.PhoneNumber, NickName = response.PhoneNumber };
                var creationResult = await _unitOfWork.UserRepository.AddUserAsync(user, response.Password);
                if (!creationResult.Succeeded)
                    return StatusCode(500);

                _logger.LogInformation($"A new User have been created [{user.UserName}].");
            }
            else
            {
                await _unitOfWork.UserRepository.UpdateUserPasswordAsync(user, response.Password);
            }

            _unitOfWork.VerificationRequestsRepository.RemoveVerificationRequest(request.Id);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Verification was succesfull for User [{user.UserName}.]");

            return Ok();
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
