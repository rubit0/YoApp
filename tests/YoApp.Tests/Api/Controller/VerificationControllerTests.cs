using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YoApp.Backend.Controllers;
using YoApp.Backend.Data;
using YoApp.Backend.Helper;
using YoApp.Backend.Services.Interfaces;
using YoApp.DataObjects.Verification;

namespace YoApp.Tests.Api.Controller
{
    public class VerificationControllerTests
    {
        private ILogger<VerificationController> _logger;

        public VerificationControllerTests()
        {
            _logger = new Mock<ILogger<VerificationController>>().Object;
        }

        [Fact]
        public async void ChallangeVerification_ReturnsBadRequestOnInvalidForm()
        {
            //Arrange
            var form = new VerificationFormDto();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(r => r.UserRepository
                .GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(null);

            var messageSenderMock = new Mock<ISmsSender>();
            var controller = new VerificationController(_logger, unitOfWorkMock.Object, messageSenderMock.Object);

            //Act
            var response = await controller.ChallengeVerification(form);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }
    }
}
