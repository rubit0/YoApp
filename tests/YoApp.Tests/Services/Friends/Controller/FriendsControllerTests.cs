using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Identity.Controllers;
using YoApp.Friends.Helper;
using YoApp.Data.Repositories;
using Microsoft.AspNetCore.Http;
using YoApp.Data.Models;
using System.Threading.Tasks;
using YoApp.DataObjects.Users;
using System.Collections.Generic;

namespace YoApp.Tests.Services.Friends.Controller
{
    public class FriendsControllerTests
    {
        private ILogger<FriendsController> _logger;

        public FriendsControllerTests()
        {
            _logger = new Mock<ILogger<FriendsController>>().Object;
        }

        [Fact]
        public async void FindFriend_OnEmptyPhoneNumber_ReturnsBadRequest()
        {
            //Arrange
            var mapperMock = new Mock<IMapper>();
            var persistenceMock = new Mock<IFriendsPersistence>();

            var controller = new FriendsController(_logger, persistenceMock.Object, mapperMock.Object);

            //Act
            var response = await controller.FindUser(string.Empty);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Theory]
        [InlineData("Bob")]
        public async void FindFriend_OnUnknownPhoneNumber_ReturnsNotFound(string phoneNumber)
        {
            //Arrange
            var mapperMock = new Mock<IMapper>();
            var persistenceMock = new Mock<IFriendsPersistence>();
            persistenceMock
                .Setup(r => r.Friends
                .FindByNameAsync(phoneNumber))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var httpContextMock = new Mock<DefaultHttpContext>(null);
            httpContextMock
                .SetupGet(am => am.User.Identity.Name)
                .Returns("Somebody");

            var controller = new FriendsController(_logger, persistenceMock.Object, mapperMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            //Act
            var respone = await controller.FindUser(phoneNumber);

            //Assert
            Assert.IsType<NotFoundResult>(respone);
        }

        [Theory]
        [InlineData("Bob")]
        public async void FindFriend_OnOkResponse_ReturnsUserDtoWithValidProperties(string phoneNumber)
        {
            //Arrange
            var fakeUser = new ApplicationUser { UserName = phoneNumber };
            var fakeDto = new UserDto { Username = phoneNumber };

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<UserDto>(fakeUser))
                .Returns(fakeDto);

            var persistenceMock = new Mock<IFriendsPersistence>();
            persistenceMock
                .Setup(r => r.Friends
                .FindByNameAsync(phoneNumber))
                .ReturnsAsync(fakeUser);

            var controller = new FriendsController(_logger, persistenceMock.Object, mapperMock.Object);

            //Act
            var respone = await controller.FindUser(phoneNumber);
            var dto = ((OkObjectResult)respone).Value as UserDto;

            //Assert
            Assert.IsType<OkObjectResult>(respone);
            Assert.IsType<UserDto>(dto);
            Assert.Equal(phoneNumber, dto.Username);
        }

        [Fact]
        public async void FindFriends_OnAnyFindingMatches_ReturnsOk()
        {
            //Arrange
            var requestPhoneNumbers = new List<string> { "123", "456", "789" };

            var fakeUsers = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "123"},
                new ApplicationUser { UserName = "456" }
            };

            var fakeDtos = new List<UserDto>
            {
                new UserDto{ Username = fakeUsers[0].UserName },
                new UserDto{ Username = fakeUsers[1].UserName },
            };

            var persistenceMock = new Mock<IFriendsPersistence>();
            persistenceMock
                .Setup(r => r.Friends
                .FindByNameRangeAsync(requestPhoneNumbers))
                .ReturnsAsync(fakeUsers);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<IEnumerable<UserDto>>(fakeUsers))
                .Returns(fakeDtos);

            var controller = new FriendsController(_logger, persistenceMock.Object, mapperMock.Object);

            //Act
            var response = await controller.FindUsers(requestPhoneNumbers);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
