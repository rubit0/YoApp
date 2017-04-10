using Microsoft.AspNetCore.Identity;
using Moq;
using YoApp.Core.Models;

namespace YoApp.Tests
{
    public static class MockHelpers
    {
        public static Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
