using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using YoApp.Backend.Controllers;
using YoApp.Backend.Data;
using YoApp.Backend.Data.EF;
using YoApp.Backend.Data.Repositories;
using YoApp.Backend.DataObjects.Account;


namespace YoApp.Tests.Api.Controller
{
    public class AccountControllerTests
    {
        private AccountController _accountController;

        public AccountControllerTests()
        {
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.IsPhoneNumberTaken("123456")).Returns(true);
            mockUserRepo.Setup(r => r.IsPhoneNumberTaken("1234567")).Returns(false);

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(m => m.UserRepository).Returns(mockUserRepo.Object);
            
            _accountController = new AccountController(mockUoW.Object);
        }

        [Fact]
        public void InitialUserCreationForm_OnTakenPhoneNumber_ShouldReturnBadRequest()
        {
            var form = new InitialUserCreationForm
            {
                CountryCode = 49,
                PhoneNumber = "123456"
            };

            var result = _accountController.InitialSetup(form);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void InitialUserCreationForm_OnUntakenPhoneNumber_ShouldReturnOk()
        {
            var form = new InitialUserCreationForm
            {
                CountryCode = 49,
                PhoneNumber = "1234567"
            };

            var result = _accountController.InitialSetup(form);

            Assert.IsType<OkResult>(result);
        }
    }
}
