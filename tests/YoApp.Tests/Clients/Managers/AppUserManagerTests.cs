using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;
using YoApp.DataObjects.Account;

namespace YoApp.Tests.Clients.Managers
{
    public class AppUserManagerTests
    {
        [Fact]
        public async void LoadUser_OnExistingUser_ReturnsUser()
        {
            //Arrange
            var store = new Mock<IKeyValueStore>();
            store.Setup(s => s.Get<AppUser>(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());

            var accountService = new Mock<IAccountService>();

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            var user = await manager.LoadUser();

            //Assert
            Assert.NotNull(user);
        }

        [Fact]
        public async void PersistUser_OnValidUser_CallsPersist()
        {
            //Arrange
            var store = new Mock<IKeyValueStore>();
            store.Setup(s => s.Get<AppUser>(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());

            var accountService = new Mock<IAccountService>();

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            var user = await manager.LoadUser();
            await manager.PersistUser();

            //Assert
            store.Verify(s => s.Insert(user), Times.AtLeastOnce);
            store.Verify(s => s.Persist(), Times.AtLeastOnce);
        }

        [Fact]
        public async void SyncUpAsync_OnDisconnected_ReturnsFalse()
        {
            //Arrange
            var store = new Mock<IKeyValueStore>();
            store.Setup(s => s.Get<AppUser>(It.IsAny<string>()))
                .ReturnsAsync(new AppUser { Nickname = "User", PhoneNumber = "123456789" });

            var accountService = new Mock<IAccountService>();
            accountService.Setup(s => s.SyncUpAsync(It.IsAny<UpdatedAccountDto>()))
                .ReturnsAsync(false);

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            await manager.LoadUser();
            var result = await manager.SyncUpAsync();

            //Assert
            accountService.Verify(s => s.SyncUpAsync(It.IsAny<UpdatedAccountDto>()), Times.AtLeastOnce);
            store.Verify(s => s.Persist(), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public async void SyncUpAsync_OnConnected_CallsPersist()
        {
            //Arrange
            var store = new Mock<IKeyValueStore>();
            store.Setup(s => s.Get<AppUser>(It.IsAny<string>()))
                .ReturnsAsync(new AppUser{Nickname = "User", PhoneNumber = "123456789"});

            var accountService = new Mock<IAccountService>();
            accountService.Setup(s => s.SyncUpAsync(It.IsAny<UpdatedAccountDto>()))
                .ReturnsAsync(true);

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            await manager.LoadUser();
            var result = await manager.SyncUpAsync();

            //Assert
            accountService.Verify(s => s.SyncUpAsync(It.IsAny<UpdatedAccountDto>()), Times.AtLeastOnce);
            store.Verify(s => s.Persist(), Times.AtLeastOnce);
            Assert.True(result);
        }

        [Theory]
        [InlineData("OldUser", "FreshUser", "YoApp is Cool!", "YoApp Rocks!")]
        public async void SyncDownAsync_OnConnected_MutatesUserPropertiesAndCallsPersist(
            string oldUserName, string newUsername, string oldStatus, string newStatus)
        {
            //Arrange
            var user = new AppUser {Nickname = oldUserName, Status = oldStatus};

            var store = new Mock<IKeyValueStore>();
            store.Setup(s => s.Get<AppUser>(It.IsAny<string>()))
                .ReturnsAsync(user);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(s => s.SyncDownAsync())
                .ReturnsAsync(new UpdatedAccountDto
                {
                    Nickname = newUsername, StatusMessage = newStatus
                });

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            await manager.LoadUser();
            var result = await manager.SyncDownAsync();

            //Assert
            accountService.Verify(s => s.SyncDownAsync(), Times.AtLeastOnce);
            store.Verify(s => s.Persist(), Times.AtLeastOnce);
            Assert.Equal(user.Nickname, newUsername);
            Assert.Equal(user.Status, newStatus);
            Assert.True(result);
        }

        [Theory]
        [InlineData("OldUser", "YoApp is Cool!")]
        public async void SyncDownAsync_OnDisconnected_ReturnsFalse(string oldUserName, string oldStatus)
        {
            //Arrange
            var user = new AppUser { Nickname = oldUserName, Status = oldStatus };

            var store = new Mock<IKeyValueStore>();
            store.Setup(s => s.Get<AppUser>(It.IsAny<string>()))
                .ReturnsAsync(user);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(s => s.SyncDownAsync())
                .Returns(Task.FromResult<UpdatedAccountDto>(null));

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            await manager.LoadUser();
            var result = await manager.SyncDownAsync();

            //Assert
            accountService.Verify(s => s.SyncDownAsync(), Times.AtLeastOnce);
            store.Verify(s => s.Persist(), Times.Never);
            Assert.Equal(user.Nickname, oldUserName);
            Assert.Equal(user.Status, oldStatus);
            Assert.False(result);
        }

        [Fact]
        public void InitUser_ValidPhoeNumber_SetsUserProperty()
        {
            //Arrange
            var phoneNumber = "123456789";

            var store = new Mock<IKeyValueStore>();
            var accountService = new Mock<IAccountService>();

            //Act
            var manager = new AppUserManager(store.Object, accountService.Object);
            manager.InitUser(phoneNumber);

            //Arrange
            Assert.Equal(manager.User.PhoneNumber, phoneNumber);
        }
    }
}
