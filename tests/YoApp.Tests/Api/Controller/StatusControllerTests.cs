using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using YoApp.Backend.Controllers;

namespace YoApp.Tests.Api.Controller
{
    public class StatusControllerTests
    {
        [Fact]
        public void VerificationOnline_ReturnsOk()
        {
            //Arrange
            var controller = new StatusController();

            //Act
            var result = controller.VerificationOnline();

            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
