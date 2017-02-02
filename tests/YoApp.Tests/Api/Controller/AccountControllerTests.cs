using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Backend.Controllers;
using YoApp.Backend.Data;
using YoApp.Backend.Models;
using Microsoft.AspNetCore.Http;


namespace YoApp.Tests.Api.Controller
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
        public async void UpdateNickname_ReturnsNewName(string username, string oldNickname, string newNickname)
        {
            //Arrange
            var fakeUser = new ApplicationUser
            {
                UserName = username,
                Nickname = oldNickname
            };
            
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUser);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(username);

            var controller = new AccountController(_logger, unitOfWorkMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var response = await controller.UpdateNickname(newNickname);
            var resultValue = ((OkObjectResult)response).Value as string;

            //Assert
            Assert.Equal(newNickname, resultValue);
        }

        [Theory]
        [InlineData("4915701234", "Bob", "Martin")]
        public async void UpdateStatus_ReturnsNewStatus(string username, string oldStatus, string newStatus)
        {
            //Arrange
            var fakeUser = new ApplicationUser
            {
                UserName = username,
                Status = oldStatus
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUser);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns(username);

            var controller = new AccountController(_logger, unitOfWorkMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var response = await controller.UpateStatus(newStatus);
            var resultValue = ((OkObjectResult)response).Value as string;

            //Assert
            Assert.Equal(newStatus, resultValue);
        }

        [Fact]
        public async void GetNickname_NullUserReturnsNotfound()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(null);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Anybody");

            var controller = new AccountController(_logger, unitOfWorkMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var response = await controller.GetNickname();

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async void GetStatus_NullUserReturnsNotfound()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(null);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Anybody");

            var controller = new AccountController(_logger, unitOfWorkMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var response = await controller.GetStatus();

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }
    }
}
