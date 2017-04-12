using System;
using Xunit;
using YoApp.Clients.Core;
using YoApp.Clients.Manager;

namespace YoApp.Tests.Clients.Managers
{
    public class VerificationManagerTests
    {
        [Fact]
        public async void RequestVerificationCode_EmptyParameters_Throws()
        {
            //Arrange
            var fakeAppsettings = new AppSettings(null)
            {
                Identity = new AppSettings.BackendHost {Host = "inernet.com"}
            };

            //Act
            var manager = new VerificationManager(fakeAppsettings);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async ()=> await manager.RequestVerificationCodeAsync(null, "   "));
        }
    }
}
