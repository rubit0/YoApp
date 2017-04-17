using System.Collections.Generic;
using Moq;
using Xunit;
using System.Reactive.Linq;
using System.Threading.Tasks;
//using Realms;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Tests.Clients.Managers
{
    public class FriendsManagerTests
    {
        [Fact]
        public void MatchFriendsToContacts_OnMatchingContacts_AssignsLocalContactToFriend()
        {
            //Arrange
            var sequence = new List<List<Friend>>();
            var fakeFriends = new List<Friend>
            {
                new Friend { PhoneNumber = "491731233456" },
                new Friend { PhoneNumber = "491731233467" }
            };
            sequence.Add(fakeFriends);
            var fakeLocalContacts = new List<LocalContact>{ new LocalContact("01731233456")};

            var storeMock = new Mock<IKeyValueStore>();
            storeMock.Setup(s => s.GetAllObservable<Friend>()).Returns(sequence.ToObservable);

            var realmMock = new Mock<IRealmStore>();
            var friendsServiceMock = new Mock<IFriendsService>();

            //Act
            var manager = new FriendsManager(storeMock.Object, realmMock.Object, friendsServiceMock.Object);
            manager.MatchFriendsToContacts(fakeFriends, fakeLocalContacts);

            //Assert
            Assert.Equal(fakeFriends[0].LocalContact, fakeLocalContacts[0]);
            Assert.Equal(fakeFriends[0].PhoneNumber, fakeLocalContacts[0].NormalizedPhoneNumber);
        }

        //[Fact]
        public async void DiscoverFriendsAsync_OnMatchingContacts_AssignsLocalContactToFriend()
        {
            //Arrange
            var sequence = new List<List<Friend>> {new List<Friend>()};
            var matchingFriend = new Friend {PhoneNumber = "491731233456"};
            var fakeLocalContacts = new List<LocalContact> { new LocalContact("01731233456"){Id = "1"} };

            var storeMock = new Mock<IKeyValueStore>();
            storeMock.Setup(s => s.GetAllObservable<Friend>()).Returns(sequence.ToObservable);

            var realmMock = new Mock<IRealmStore>();
            //realmMock.Setup(r => r.AddAsync(It.IsAny<RealmObject>())).Returns(Task.CompletedTask);

            var friendsServiceMock = new Mock<IFriendsService>();
            friendsServiceMock.Setup(f => f.CheckMembership(matchingFriend.PhoneNumber)).ReturnsAsync(true);
            friendsServiceMock.Setup(f => f.FetchFriend(matchingFriend.PhoneNumber)).ReturnsAsync(matchingFriend);

            //Act
            var manager = new FriendsManager(storeMock.Object, realmMock.Object, friendsServiceMock.Object);
            await manager.DiscoverFriendsAsync(fakeLocalContacts);

            //Assert
            Assert.Equal(matchingFriend.LocalContact, fakeLocalContacts[0]);
            Assert.Equal(manager.Friends[0], matchingFriend);
        }
    }
}
