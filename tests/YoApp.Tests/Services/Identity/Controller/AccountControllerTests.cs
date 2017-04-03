using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Identity.Controllers;
using YoApp.Data.Models;
using YoApp.DataObjects.Account;
using System.Threading.Tasks;

namespace YoApp.Tests.Services.Identity.Controller
{
    public class AccountControllerTests
    {
        private ILogger<AccountController> _logger;

        public AccountControllerTests()
        {
            _logger = new Mock<ILogger<AccountController>>().Object;
        }

        [Theory]
        [InlineData("4915701234", "Bob", "Martin")]
        public async void UpdateName_OnNicknameUpdate_ReturnsOk(string username, string oldNickname, string newNickname)
        {
            //Arrange
            var fakeUser = new ApplicationUser
            {
                UserName = username,
                Nickname = oldNickname
            };

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUser);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(username);

            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var dto = new UpdatedAccountDto { Nickname = newNickname };
            var response = await controller.UpdateAccount(dto);



            //Assert
            Assert.IsType<OkResult>(response);
        }

        [Theory]
        [InlineData("4915701234", "Bob", "Martin")]
        public async void UpdateStatus_OnStatusUpdate_ReturnsOkStatus(string username, string oldStatus, string newStatus)
        {
            //Arrange
            var fakeUser = new ApplicationUser
            {
                UserName = username,
                Status = oldStatus
            };

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUser);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(username);

            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var dto = new UpdatedAccountDto { StatusMessage = newStatus };
            var response = await controller.UpdateAccount(dto);

            //Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public async void GetNickname_NullUser_ReturnsNotfound()
        {
            //Arrange
            var unitOfWorkMock = MockHelpers.GetMockUserManager();
            unitOfWorkMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Anybody");

            var controller = new AccountController(_logger, unitOfWorkMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var response = await controller.GetAccount();
            var codeResult = ((StatusCodeResult)response).StatusCode;

            //Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(codeResult, 500);
        }

        [Fact]
        public async void GetStatus_NullUser_ReturnsNotfound()
        {
            //Arrange
            var mockUserManager = MockHelpers.GetMockUserManager();
            mockUserManager
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Anybody");

            var controller = new AccountController(_logger, mockUserManager.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var response = await controller.GetAccount();
            var codeResult = ((StatusCodeResult)response).StatusCode;

            //Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(codeResult, 500);
        }
    }
}
