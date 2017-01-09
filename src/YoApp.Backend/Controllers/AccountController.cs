using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoApp.Backend.Data;
using YoApp.Backend.DataObjects.Account;
using YoApp.Backend.Services.Interfaces;

namespace YoApp.Backend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageSender _messageSender;

        public AccountController(IUnitOfWork unitOfWork, IMessageSender messageSender)
        {
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

            if (_unitOfWork.UserRepository.IsPhoneNumberTaken(form.PhoneNumber))
                return BadRequest("Phonenumber already taken");

            var number = $"+{form.CountryCode}{form.PhoneNumber}";
            await _messageSender.SendMessageAsync(number, "Hello from YoApp!");

            return Ok();
        }
    }
}
