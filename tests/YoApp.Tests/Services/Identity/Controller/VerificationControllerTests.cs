using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.DataObjects.Verification;
using YoApp.Identity.Controllers;
using YoApp.Identity;
using YoApp.Identity.Services.Interfaces;
using YoApp.Identity.Helper;
using Microsoft.AspNetCore.Identity;
using YoApp.Data.Models;

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
        public async void RequestVerification_OnBlackListedCountryCode_ReturnsBadRequest()
        {
            //Arrange
            var form = new VerificationChallengeDto
            {
                CountryCode = "49",
                PhoneNumber = "1730456789"
            };

            var unitOfWorkMock = new Mock<Persistence>();
            var messageSenderMock = new Mock<ISmsSender>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>();

            var configurationMock = new Mock<IConfigurationService>();
            var validCountryCode = new List<int> { 1, 11, 111 };
            configurationMock
                .SetupGet(c => c.CountriesBlackList).Returns(validCountryCode);

            var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object, configurationMock.Object, userManagerMock.Object);

            //Act
            var response = await controller.RequestVerification(form);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        //[Fact]
        //public async void ChallengeVerification_OnSmsSendingFailure_ReturnsStatusCode500()
        //{
        //    //Arrange
        //    var form = new VerificationChallengeDto
        //    {
        //        CountryCode = "49",
        //        PhoneNumber = "1730456789"
        //    };

        //    var unitOfWorkMock = new Mock<IUnitOfWork>();
        //    var messageSenderMock = new Mock<ISmsSender>();
        //    messageSenderMock
        //        .Setup(ms => ms.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
        //        .ReturnsAsync(false);

        //    var configurationMock = new Mock<IConfigurationService>();
        //    var validCountryCode = new List<int> { 49 };
        //    configurationMock
        //        .SetupGet(c => c.ValidCountryCallCodes).Returns(validCountryCode);

        //    var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object, configurationMock.Object);

        //    //Act
        //    var response = await controller.ChallengeVerification(form);

        //    //Assert
        //    Assert.IsType<StatusCodeResult>(response);
        //}

        //[Fact]
        //public async void ChallengeVerification_OnValidForm_ReturnsOk()
        //{
        //    //Arrange
        //    var form = new VerificationChallengeDto
        //    {
        //        CountryCode = "49",
        //        PhoneNumber = "1730456789"
        //    };

        //    var unitOfWorkMock = new Mock<IUnitOfWork>();
        //    unitOfWorkMock
        //        .Setup(r => r.VerificationRequestsRepository
        //        .FindByPhone(It.IsAny<string>()));

        //    unitOfWorkMock
        //        .Setup(r => r.VerificationRequestsRepository
        //        .AddAsync(It.IsAny<VerificationtRequest>()))
        //        .Returns(Task.FromResult(false));

        //    var messageSenderMock = new Mock<ISmsSender>();
        //    messageSenderMock
        //        .Setup(ms => ms.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
        //        .ReturnsAsync(true);

        //    var configurationMock = new Mock<IConfigurationService>();
        //    var validCountryCode = new List<int> { 49 };
        //    configurationMock
        //        .SetupGet(c => c.ValidCountryCallCodes).Returns(validCountryCode);

        //    var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object, configurationMock.Object);

        //    //Act
        //    var response = await controller.ChallengeVerification(form);

        //    //Assert
        //    Assert.IsType<OkResult>(response);
        //}

        //[Fact]
        //public async void ResolveVerification_OnNullVerificationRequests_ReturnsBadRequest()
        //{
        //    //Arrange
        //    var verificationResponseDto = new VerificationResolveDto
        //    {
        //        PhoneNumber = "491736890",
        //        Password = "123456789",
        //        VerificationCode = "789-789"
        //    };

        //    var unitOfWorkMock = new Mock<IUnitOfWork>();
        //    unitOfWorkMock
        //        .Setup(r => r.VerificationRequestsRepository
        //        .FindByPhoneAsync(It.IsAny<string>()))
        //        .ReturnsAsync(null);

        //    var messageSenderMock = new Mock<ISmsSender>();
        //    var configurationMock = new Mock<IConfigurationService>();

        //    var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object, configurationMock.Object);

        //    //Act
        //    var response = await controller.ResolveVerification(verificationResponseDto);

        //    //Assert
        //    Assert.IsType<BadRequestObjectResult>(response);
        //}

        //[Fact]
        //public async void ResolveVerification_OnNotMatchingCode_ReturnsBadRequest()
        //{
        //    //Arrange
        //    var verificationResponseDto = new VerificationResolveDto
        //    {
        //        PhoneNumber = "491736890",
        //        Password = "123456789",
        //        VerificationCode = "123-456"
        //    };

        //    var requestDto = new VerificationtRequest { VerificationCode = "456-789" };

        //    var unitOfWorkMock = new Mock<IUnitOfWork>();
        //    unitOfWorkMock
        //        .Setup(r => r.VerificationRequestsRepository
        //        .FindByPhoneAsync(It.IsAny<string>()))
        //        .ReturnsAsync(requestDto);

        //    var messageSenderMock = new Mock<ISmsSender>();
        //    var configurationMock = new Mock<IConfigurationService>();

        //    var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object, configurationMock.Object);

        //    //Act
        //    var response = await controller.ResolveVerification(verificationResponseDto);

        //    //Assert
        //    Assert.IsType<BadRequestObjectResult>(response);
        //}

        //[Fact]
        //public async void ResolveVerification_OnMatchingCode_ReturnsOk()
        //{
        //    //Arrange
        //    var verificationResponseDto = new VerificationResolveDto
        //    {
        //        PhoneNumber = "491736890",
        //        Password = "123456789",
        //        VerificationCode = "123-456"
        //    };

        //    var requestDto = new VerificationtRequest
        //    {
        //        PhoneNumber = "491736890",
        //        VerificationCode = "123-456"
        //    };

        //    var appUser = new ApplicationUser();

        //    var unitOfWorkMock = new Mock<IUnitOfWork>();
        //    unitOfWorkMock
        //        .Setup(r => r.VerificationRequestsRepository
        //        .FindByPhoneAsync(It.IsAny<string>()))
        //        .ReturnsAsync(requestDto);

        //    unitOfWorkMock
        //        .Setup(r => r.UserRepository.GetByUsernameAsync(It.IsAny<string>()))
        //        .ReturnsAsync(appUser);
        //    unitOfWorkMock.Setup(r => r.VerificationRequestsRepository.RemoveById(It.IsAny<int>()));

        //    var messageSenderMock = new Mock<ISmsSender>();
        //    var configurationMock = new Mock<IConfigurationService>();

        //    var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object, configurationMock.Object);

        //    //Act
        //    var response = await controller.ResolveVerification(verificationResponseDto);

        //    //Assert
        //    Assert.IsType<OkResult>(response);
        //}

        //[Theory]
        //[InlineData(50)]
        //public void GenerateVerificationCode_ReturnsValidCodes(int times)
        //{
        //    //Arrange
        //    var result = true;

        //    //Act
        //    for (int i = 0; i < times; i++)
        //    {
        //        var code = Utils.Misc.CodeGenerator.GetCode();
        //        if (code.Length < 6)
        //        {
        //            result = false;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.True(result);
        //}
    }
}
