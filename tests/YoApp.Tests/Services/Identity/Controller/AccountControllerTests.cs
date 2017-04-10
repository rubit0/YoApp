using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Identity.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YoApp.Core.Models;
using YoApp.Dtos.Account;

namespace YoApp.Tests.Services.Identity.Controller
{
    public class AccountControllerTests
    {
        private ILogger<AccountController> _logger;

        public AccountControllerTests()
        {
            _logger = new Mock<ILogger<AccountController>>().Object;
        }

        [Fact]
        public async void GetAccount_OnValidUserContext_OkWithDto()
        {
            //Arrange
            var user = new ApplicationUser { UserName = "123456789", Nickname = "Nick" };
            var dto = new UpdatedAccountDto { Nickname = user.Nickname };
            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(u => u.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .Setup(h => h.User.Identity.Name)
                .Returns(user.UserName);

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.GetAccount();
            var responseDto = ((OkObjectResult)response).Value as UpdatedAccountDto;

            //Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(dto.Nickname, responseDto.Nickname);
        }

        [Fact]
        public async void GetAccount_OnInvalidUserContext_StatusCode500()
        {
            //Arrange
            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .Setup(h => h.User.Identity.Name)
                .Returns("");

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.GetAccount();
            var statusCode = ((StatusCodeResult)response).StatusCode;

            //Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(500, statusCode);
        }

        [Theory]
        [InlineData("4915701234", "Bob", "Martin", "I Mock you!", "YoApp rocks!")]
        public async void UpdateAccount_OnUpdateNicknameAndStatuts_OkStatus(string username, 
            string oldNickname, string newNickname, string oldStatus, string newStatus)
        {
            //Arrange
            var fakeUser = new ApplicationUser
            {
                UserName = username,
                Nickname = oldNickname,
                Status = oldStatus
            };

            var identityResult = IdentityResult.Success;

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUser);

            userManagerMock
                .Setup(m => m.UpdateAsync(fakeUser))
                .ReturnsAsync(identityResult);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(username);

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var dto = new UpdatedAccountDto { Nickname = newNickname, StatusMessage = newStatus };
            var response = await controller.UpdateAccount(dto);

            //Assert
            Assert.IsType<OkResult>(response);
        }

        [Theory]
        [InlineData("4915701234", "Bob", "Martin")]
        public async void UpdateAccount_OnStatusUpdate_OkStatus(string username, string oldStatus, string newStatus)
        {
            //Arrange
            var user = new ApplicationUser
            {
                UserName = username,
                Status = oldStatus
            };

            var identityResult = IdentityResult.Success;

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(m => m.UpdateAsync(user))
                .ReturnsAsync(identityResult);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(username);

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var dto = new UpdatedAccountDto { StatusMessage = newStatus };
            var response = await controller.UpdateAccount(dto);

            //Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public async void GetNickname_NullUser_Notfound()
        {
            //Arrange
            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Anybody");

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.GetAccount();
            var codeResult = ((StatusCodeResult)response).StatusCode;

            //Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(codeResult, 500);
        }

        [Theory]
        [InlineData("Bill", "Gates")]
        public async void UpdateNickname_ValidUser_Ok(string oldName, string newName)
        {
            //Arrange
            var user = new ApplicationUser
            {
                UserName = "123456789",
                Nickname = oldName,
            };

            var identityResult = IdentityResult.Success;

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(r => r.UpdateAsync(user))
                .ReturnsAsync(identityResult);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(user.UserName);

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.UpdateName(newName);

            //Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public async void UpdateNickname_NullUser_StatusCode500()
        {
            //Arrange
            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Anybody");

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.GetAccount();
            var codeResult = ((StatusCodeResult)response).StatusCode;

            //Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(codeResult, 500);
        }

        [Fact]
        public async void GetStatus_NullUser_Notfound()
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

            //Act
            var controller = new AccountController(_logger, mockUserManager.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.GetAccount();
            var codeResult = ((StatusCodeResult)response).StatusCode;

            //Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(codeResult, 500);
        }

        [Theory]
        [InlineData("I mock you!", "YoApp Rocks!")]
        public async void UpdateStatus_ValidUser_Ok(string oldStatus, string newStatus)
        {
            //Arrange
            var user = new ApplicationUser
            {
                UserName = "123456789",
                Status = oldStatus,
            };

            var identityResult = IdentityResult.Success;

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(r => r.UpdateAsync(user))
                .ReturnsAsync(identityResult);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(user.UserName);

            //Act
            var controller = new AccountController(_logger, userManagerMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            var response = await controller.UpdateStatus(newStatus);

            //Assert
            Assert.IsType<OkResult>(response);
        }
    }
}
