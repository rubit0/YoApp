using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Identity.Controllers;
using YoApp.Identity;
using YoApp.Identity.Services.Interfaces;
using System;
using YoApp.Core.Models;
using YoApp.Dtos.Verification;
using YoApp.Identity.Core;

namespace YoApp.Tests.Services.Identity.Controller
{
    public class VerificationControllerTests
    {
        private ILogger<VerificationController> _logger;

        public VerificationControllerTests()
        {
            _logger = new Mock<ILogger<VerificationController>>().Object;
        }

        [Fact]
        public async void RequestVerification_OnBlackListedCountryCode_BadRequest()
        {
            //Arrange
            var form = new VerificationChallengeDto
            {
                CountryCode = "49",
                PhoneNumber = "1730456789"
            };

            var persistenceMock = new Mock<IIdentityPersistence>();
            var messageSenderMock = new Mock<ISmsSender>();
            var userManagerMock = MockHelpers.GetMockUserManager();

            var configurationMock = new Mock<IConfigurationService>();
            configurationMock
                .SetupGet(c => c.CountriesBlackList).Returns(new List<int> { 1, 12, 49 });

            //Act
            var controller = new VerificationController(_logger, persistenceMock.Object, 
                messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            var response = await controller.RequestVerification(form);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async void RequestVerification_OnSmsSendingFailure_StatusCode500()
        {
            //Arrange
            var form = new VerificationChallengeDto
            {
                PhoneNumber = "1730456789",
                CountryCode = "49"
            };

            var userManagerMock = MockHelpers.GetMockUserManager();
            var persistenceMock = new Mock<IIdentityPersistence>();
            var messageSenderMock = new Mock<ISmsSender>();
            messageSenderMock
                .Setup(ms => ms.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var configurationMock = new Mock<IConfigurationService>();
            configurationMock
                .SetupGet(c => c.CountriesBlackList).Returns(new int[]{ });

            //Act
            var controller = new VerificationController(_logger, persistenceMock.Object, 
                messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            var response = await controller.RequestVerification(form);

            //Assert
            Assert.IsType<StatusCodeResult>(response);
        }

        [Fact]
        public async void RequestVerification_OnValidForm_Ok()
        {
            //Arrange
            var form = new VerificationChallengeDto
            {
                CountryCode = "49",
                PhoneNumber = "1730456789"
            };

            var userManagerMock = MockHelpers.GetMockUserManager();
            var persistenceMock = new Mock<IIdentityPersistence>();
            persistenceMock
                .Setup(r => r.VerificationTokens
                .FindByUserAsync(It.IsAny<string>()));

            persistenceMock
                .Setup(r => r.VerificationTokens
                .AddAsync(It.IsAny<VerificationToken>()))
                .Returns(Task.FromResult(false));

            var messageSenderMock = new Mock<ISmsSender>();
            messageSenderMock
                .Setup(ms => ms.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var configurationMock = new Mock<IConfigurationService>();
            configurationMock
                .SetupGet(c => c.CountriesBlackList).Returns(new int[] { });

            //Act
            var controller = new VerificationController(_logger, persistenceMock.Object, 
                messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            var response = await controller.RequestVerification(form);

            //Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public async void ResolveVerification_OnNullVerificationRequests_BadRequest()
        {
            //Arrange
            var verificationResponseDto = new VerificationResolveDto
            {
                PhoneNumber = "491736890",
                Password = "123456789",
                VerificationCode = "789-789"
            };

            var userManagerMock = MockHelpers.GetMockUserManager();
            var persistenceMock = new Mock<IIdentityPersistence>();
            persistenceMock
                .Setup(r => r.VerificationTokens
                .FindByUserAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<VerificationToken>(null));

            var messageSenderMock = new Mock<ISmsSender>();
            var configurationMock = new Mock<IConfigurationService>();

            //Act
            var controller = new VerificationController(_logger, persistenceMock.Object, 
                messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            var response = await controller.ResolveVerification(verificationResponseDto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async void ResolveVerification_OnNotMatchingCode_BadRequest()
        {
            //Arrange
            var resolveDto = new VerificationResolveDto
            {
                PhoneNumber = "491736890",
                Password = "123456789",
                VerificationCode = "123456"
            };

            var token = new VerificationToken { Expires = DateTime.MaxValue };

            var userManagerMock = MockHelpers.GetMockUserManager();
            var persistenceMock = new Mock<IIdentityPersistence>();
            persistenceMock
                .Setup(r => r.VerificationTokens
                .FindByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(token);

            var messageSenderMock = new Mock<ISmsSender>();
            var configurationMock = new Mock<IConfigurationService>();

            //Act
            var controller = new VerificationController(_logger, persistenceMock.Object, 
                messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            var response = await controller.ResolveVerification(resolveDto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async void ResolveVerification_OnMatchingCode_Ok()
        {
            //Arrange
            var verificationResponseDto = new VerificationResolveDto
            {
                PhoneNumber = "491736890",
                Password = "123456789",
                VerificationCode = "123456"
            };

            var token = new VerificationToken
            {
                User = "491736890",
                Code = "123456",
                Expires = DateTime.MaxValue
            };

            var appUser = new ApplicationUser();

            var persistenceMock = new Mock<IIdentityPersistence>();
            persistenceMock
                .Setup(r => r.VerificationTokens
                .FindByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(token);

            persistenceMock
                .Setup(r => r.VerificationTokens.Remove(token));

            var userManagerMock = MockHelpers.GetMockUserManager();
            userManagerMock
                .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(appUser);

            var messageSenderMock = new Mock<ISmsSender>();
            var configurationMock = new Mock<IConfigurationService>();

            //Act
            var controller = new VerificationController(_logger, persistenceMock.Object, 
                messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            var response = await controller.ResolveVerification(verificationResponseDto);

            //Assert
            Assert.IsType<OkResult>(response);
        }
    }
}
