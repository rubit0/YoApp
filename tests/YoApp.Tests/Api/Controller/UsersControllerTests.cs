using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Backend.Controllers;
using YoApp.Backend.Data;
using YoApp.Backend.Models;
using YoApp.DataObjects.Users;
using YoApp.DataObjects.Account;

namespace YoApp.Tests.Api.Controller
{
    public class UsersControllerTests
    {
        private ILogger<FriendsController> _logger;

        public UsersControllerTests()
        {
            _logger = new Mock<ILogger<FriendsController>>().Object;
        }

        [Fact]
        public async void GetUser_OnEmptyPhoneNumber_ReturnsBadRequest()
        {
            //Arrange
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var controller = new FriendsController(_logger, unitOfWorkMock.Object,mapperMock.Object);

            //Act
            var response = await controller.GetUser(string.Empty);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Theory]
        [InlineData("Bob")]
        public async void GetUser_OnUnknownPhoneNumber_ReturnsNotFound(string phoneNumber)
        {
            //Arrange
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetByUsernameAsync(phoneNumber))
                .ReturnsAsync(null);

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Somebody");

            var controller = new FriendsController(_logger, unitOfWorkMock.Object, mapperMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var respone = await controller.GetUser(phoneNumber);

            //Assert
            Assert.IsType<NotFoundResult>(respone);
        }

        [Theory]
        [InlineData("Bob")]
        public async void GetUser_OnOkResponse_ReturnsUser(string phoneNumber)
        {
            //Arrange
            var fakeUser = new ApplicationUser { UserName = phoneNumber };
            var fakeDto = new CreatedAccountDto { PhoneNumber = phoneNumber };

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<CreatedAccountDto>(fakeUser))
                .Returns(fakeDto);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetByUsernameAsync(phoneNumber))
                .ReturnsAsync(fakeUser);

            var controller = new FriendsController(_logger, unitOfWorkMock.Object, mapperMock.Object);

            //Act
            var respone = await controller.GetUser(phoneNumber);
            var dto = ((OkObjectResult) respone).Value as CreatedAccountDto;

            //Assert
            Assert.Equal(phoneNumber, dto.PhoneNumber);
        }

        [Fact]
        public async void GetUsers_OnAnyFindingMatche_ReturnsOk()
        {
            //Arrange
            var requestPhoneNumbers = new List<string> {"123", "456", "789"};

            var fakeUsers = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "123"},
                new ApplicationUser { UserName = "456" }
            };

            string[] x = new[] {"", ""};

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetByUsernamesAsync(requestPhoneNumbers))
                .ReturnsAsync(fakeUsers);

            var mapperMock = new Mock<IMapper>();

            var controller = new FriendsController(_logger, unitOfWorkMock.Object, mapperMock.Object);

            //Act
            var response = await controller.GetUsers(requestPhoneNumbers);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
