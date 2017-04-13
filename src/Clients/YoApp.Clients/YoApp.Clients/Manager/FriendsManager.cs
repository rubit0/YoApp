using Plugin.Connectivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YoApp.Clients.Core;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients.Manager
{
    public class FriendsManager : IFriendsManager
    {
        public ObservableCollection<Friend> Friends { get; private set; }

        private readonly IKeyValueStore _keyValueStore;
        private readonly IRealmStore _realmStore;
        private readonly IFriendsService _friendsService;

        public FriendsManager(IKeyValueStore keyValueStore, IRealmStore realmStore, IFriendsService friendsService)
        {
            _keyValueStore = keyValueStore;
            _realmStore = realmStore;
            _friendsService = friendsService;
        }

        public async Task LoadFriends()
        {
            var friendsFromStore = await _keyValueStore.GetAll<Friend>() ?? new List<Friend>();
            Friends = new ObservableCollection<Friend>(friendsFromStore);
        }

        /// <summary>
        /// Handles all relevant aspects of managing friends.
        /// </summary>
        public async Task ManageFriends(List<LocalContact> contacts)
        {
            if(Friends == null || contacts == null)
                return;

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
            await _keyValueStore.Persist();
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
            var unassociatedContacts = contacts
                .Except(contactsFromFriends, new ContactComparer()).ToList();

            var friends = await _friendsService
                .FetchFriends(unassociatedContacts.Select(f => f.NormalizedPhoneNumber));

            foreach (var friend in friends)
            {
                if (Friends.Contains(friend, new FriendsComparer()))
                    continue;

                friend.LocalContact = unassociatedContacts.SingleOrDefault(c => c.NormalizedPhoneNumber == friend.PhoneNumber);

                //Persist friend
                await _keyValueStore.Insert(friend);

                Friends.Add(friend);

                //Persist chatbook
                var chatbook = new ChatBook() { FriendKey = friend.Key };
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
                await _keyValueStore.Remove(friend);
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
