using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Sdk;
using YoApp.Backend.Controllers;
using YoApp.Backend.Data;
using YoApp.Backend.Data.EF;
using YoApp.Backend.Data.Repositories;
using YoApp.Backend.Services.Interfaces;


namespace YoApp.Tests.Api.Controller
{
    public class AccountControllerTests
    {
        private ILogger<AccountController> _logger;

        public AccountControllerTests()
        {
            _logger = new Mock<ILogger<AccountController>>().Object;
        }

        //[Fact]
        //public void InitialUserCreationForm_OnInvalidForm_ShouldReturnBadRequest()
        //{
        //    var messageSenderMock = new Mock<IMessageSender>();
        //    var userRepoMock = new Mock<IUnitOfWork>();
        //    userRepoMock.Setup(r => r.UserRepository.IsPhoneNumberTaken("+49123456")).Returns(true);
        //    var accountController = new AccountController(_logger, userRepoMock.Object, messageSenderMock.Object);
            
        //    var form = new InitialVerificationForm
        //    {
        //        CountryCode = 0,
        //        PhoneNumber = null
        //    };

        //    var result = accountController.ChallengeVerification(form).Result;
        //    Assert.IsType<BadRequestResult>(result);
        //}
    }
}
