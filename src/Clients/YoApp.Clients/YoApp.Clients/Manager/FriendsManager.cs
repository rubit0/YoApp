using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YoApp.Clients.Helpers;
using YoApp.Clients.Helpers.Extensions;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.Manager
{
    public class FriendsManager : IFriendsManager
    {
        public ObservableCollection<Friend> Friends { get; private set; }

        private readonly IKeyValueStore _store;
        private readonly IRealmStore _realmStore;
        private readonly IFriendsService _friendsService;

        public FriendsManager()
        {
            _store = App.Persistence.Resolve<IKeyValueStore>();
            _realmStore = App.Persistence.Resolve<IRealmStore>();
            _friendsService = App.Services.Resolve<IFriendsService>();

            _store.GetAllObservable<Friend>()
                .Subscribe(friends =>
                {
                    if (friends == null)
                        friends = new List<Friend>();

                    Friends = new ObservableCollection<Friend>(friends);
                });
        }

        /// <summary>
        /// Handles all relevant aspects of managing friends.
        /// </summary>
        public async Task ManageFriends(List<LocalContact> contacts)
        {
            //Get rid of all friends that can't be associated with a local contact
            await RemoveStaleFriendsAsync(contacts);

            //Get contact-less friends
            var looseFriends = Friends
                .Where(f => f.LocalContact == null)
                .ToList();

            MatchFriendsToContacts(looseFriends, contacts);

            if (CrossConnectivity.Current.IsConnected 
                && App.Settings.SetupFinished)
                await DiscoverFriendsAsync(contacts);

            //Persist
            await _store.Persist();
        }

        /// <summary>
        /// Match Friends to the given contacts by PhoneNumber.
        /// </summary>
        /// <param name="friends">Friends that have contacts</param>
        /// <param name="contacts">Contacts to match on</param>
        public void MatchFriendsToContacts(IEnumerable<Friend> friends, List<LocalContact> contacts)
        {
            foreach (var friend in friends)
            {
                friend.LocalContact = contacts.SingleOrDefault(c =>
                {
                    if (!c.IsValidPhoneNumber)
                        return false;

                    return c.NormalizedPhoneNumber == friend.Key;
                });
            }
        }

        /// <summary>
        /// Discover new matching friends from the backend.
        /// </summary>
        public async Task DiscoverFriendsAsync(List<LocalContact> contacts)
        {
            //Find unassociated contacts
            var contactsFromFriends = Friends.Select(f => f.LocalContact);
            var unassociatedContacts = contacts.Except(contactsFromFriends, new ContactComparer());

            foreach (var contact in unassociatedContacts)
            {
                if (!contact.IsValidPhoneNumber)
                    continue;

                if (!await _friendsService.CheckMembership(contact.NormalizedPhoneNumber))
                    continue;

                var friend = await _friendsService.FetchFriend(contact.NormalizedPhoneNumber);
                if (friend == null)
                    continue;

                friend.LocalContact = contact;

                //Persist friend
                await _store.Insert(friend);

                Friends.Add(friend);

                //Persist chatbook
                var chatbook = new ChatBook { FriendKey = friend.Key };
                await _realmStore.AddAsync(chatbook);

                OnPropertyChanged(nameof(Friends));
            }
        }

        /// <summary>
        /// Remove friends that aren't found on the given contacts
        /// </summary>
        /// <param name="contacts">Source to look at.</param>
        public async Task RemoveStaleFriendsAsync(List<LocalContact> contacts)
        {
            var stale = Friends
                .Where(f => contacts.All(c => c.NormalizedPhoneNumber != f.Key))
                .ToList();

            foreach (var friend in stale)
            {
                await _store.Remove(friend);
                Friends.Remove(friend);

                //Remove Chatbook
                var chatbook = _realmStore.Find<ChatBook>(friend.Key);
                if (chatbook != null)
                    await _realmStore.Remove(chatbook);

                OnPropertyChanged(nameof(Friends));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
