using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoApp.Backend.Data;
using YoApp.Backend.DataObjects.Account;

namespace YoApp.Backend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }

        [HttpPost("InitialSetup")]
        public IActionResult InitialSetup(InitialUserCreationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_unitOfWork.UserRepository.IsPhoneNumberTaken(form.PhoneNumber))
                return BadRequest("Phonenumber already taken");

            return Ok();
        }
    }
}
