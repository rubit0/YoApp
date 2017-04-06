using System.Collections.Generic;
using Moq;
using Xunit;
using System.Reactive.Linq;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Tests.Clients.Managers
{
    public class FriendsManagerTests
    {
        [Fact]
        public async void ManageFriends_OnProvidedContacts_CallsAllMethods()
        {
            //Arrange
            var root = new List<List<Friend>>();
            var fakeFriends = new List<Friend>{ new Friend { PhoneNumber = "123" }, new Friend { PhoneNumber = "456" } };
            root.Add(fakeFriends);

            var storeMock = new Mock<IKeyValueStore>();
            storeMock.Setup(s => s.GetAllObservable<Friend>()).Returns(root.ToObservable);

            var realmMock = new Mock<IRealmStore>();
            var friendsServiceMock = new Mock<IFriendsService>();

            var contacts = new List<LocalContact>{ new LocalContact("123") };

            //Act
            var manager = new FriendsManager(storeMock.Object, realmMock.Object, friendsServiceMock.Object);
            await manager.ManageFriends(contacts);

            //Assert

        }
    }
}
