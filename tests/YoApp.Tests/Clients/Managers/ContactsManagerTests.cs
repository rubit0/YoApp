using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Plugin.Contacts.Abstractions;
using Xunit;
using YoApp.Clients.Manager;

namespace YoApp.Tests.Clients.Managers
{
    public class ContactsManagerTests
    {
        [Fact]
        public async void LoadContactsAsync_OnNoPermission_ReturnsFalse()
        {
            //Arrange
            var deviceContactsMock = new Mock<IContacts>();
            deviceContactsMock.Setup(d => d.RequestPermission()).ReturnsAsync(false);

            //Act
            var manager = new ContactsManager(deviceContactsMock.Object);
            var result = await manager.LoadContactsAsync();

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void LoadContactsAsync_OnSameLoadedContacts_ReturnsTrueAndFillsProperty()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact("1", false)
                {
                    DisplayName = "Dummy",
                    Phones = new List<Phone> {new Phone()}
                }
            };
            var deviceContactsMock = new Mock<IContacts>();
            deviceContactsMock.Setup(d => d.RequestPermission()).ReturnsAsync(true);
            deviceContactsMock.Setup(d => d.Contacts).Returns(() => contacts.AsQueryable());

            //Act
            var manager = new ContactsManager(deviceContactsMock.Object);
            var result = await manager.LoadContactsAsync();

            //Assert
            Assert.True(result);
            Assert.Equal(contacts.Count, manager.Contacts.Count);
        }


        [Fact]
        public async void LoadContactsAsync_OnSameLoadedAndFetchedContacts_ReturnsFalse()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact("1", false)
                {
                    DisplayName = "Dummy",
                    Phones = new List<Phone> {new Phone()}
                }
            };
            var deviceContactsMock = new Mock<IContacts>();
            deviceContactsMock.Setup(d => d.RequestPermission()).ReturnsAsync(true);
            deviceContactsMock.Setup(d => d.Contacts).Returns(() => contacts.AsQueryable());

            //Act
            var manager = new ContactsManager(deviceContactsMock.Object);
            await manager.LoadContactsAsync();
            //second run
            var result = await manager.LoadContactsAsync();

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void BuildContactGroup_OnContacts_ReturnsValidObject()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact("1", false)
                {
                    DisplayName = "Dummy",
                    Phones = new List<Phone> {new Phone()}
                }
            };
            var deviceContactsMock = new Mock<IContacts>();
            deviceContactsMock.Setup(d => d.RequestPermission()).ReturnsAsync(true);
            deviceContactsMock.Setup(d => d.Contacts).Returns(() => contacts.AsQueryable());

            //Act
            var manager = new ContactsManager(deviceContactsMock.Object);
            var result = await manager.LoadContactsAsync();
            var groups = manager.BuildContactGroup();

            //Assert
            Assert.True(result);
            Assert.NotNull(groups);
        }
    }
}
