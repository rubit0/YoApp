using Plugin.Contacts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Plugin.Contacts.Abstractions;
using YoApp.Clients.Helpers;
using YoApp.Clients.Models;
using YoApp.Clients.ViewModels.ListViewGroups;

namespace YoApp.Clients.Manager
{
    /// <summary>
    /// Manage local contacts and match to friends
    /// </summary>
    public class ContactsManager : IContactsManager
    {
        public List<LocalContact> Contacts
        {
            get => _contacts;
            private set
            {
                _contacts = value;
                OnPropertyChanged();
            }
        }

        private List<LocalContact> _contacts;
        private readonly ContactComparer _contactComparer;
        private readonly IContacts _deviceContacts;

        public ContactsManager(IContacts deviceContacts)
        {
            _deviceContacts = deviceContacts;
            _contacts = new List<LocalContact>();
            _contactComparer = new ContactComparer();
        }

        /// <summary>
        /// Load new Contacts from the device contact book.
        /// </summary>
        /// <returns>Has been new contacts found?</returns>
        public async Task<bool> LoadContactsAsync()
        {
            var hasPermission = await _deviceContacts.RequestPermission();
            if (!hasPermission)
                return false;

            _deviceContacts.PreferContactAggregation = false;

            if (_deviceContacts.Contacts?.ToArray().Length
                == Contacts?.Count)
                return false;

            Contacts = await Task.Run(() =>
            {
                var fetchedContacts = _deviceContacts
                    .Contacts
                    .ToArray()
                    .Where(c => !string.IsNullOrWhiteSpace(c.DisplayName)
                                && c.Phones.Count > 0)
                    .SelectMany(c =>
                    {
                        return c.Phones.Select(phone =>
                            new LocalContact(phone.Number)
                            {
                                Id = c.Id,
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                DisplayName = c.DisplayName,
                                Label = phone.Label
                            });
                    })
                    .ToList();

                fetchedContacts.Sort(_contactComparer);

                return fetchedContacts;
            });

            return true;
        }

        /// <summary>
        /// Get Contacts grouped by alphabet.
        /// </summary>
        /// <returns>Grouped Contacts</returns>
        public List<ContactGroup> BuildContactGroup()
        {
            return BuildContactGroup(Contacts);
        }

        /// <summary>
        /// Get Contacts grouped by alphabet.
        /// </summary>
        /// <param name="contacts">Target Contacts</param>
        /// <returns>Grouped Contacts</returns>
        public List<ContactGroup> BuildContactGroup(IEnumerable<LocalContact> contacts)
        {
            var superGroup = new List<ContactGroup>();
            var groups = contacts
                .GroupBy(c => char.ToUpper(c.GetSortFlag()))
                .OrderBy(c => c.Key)
                .ToList();

            foreach (var group in groups)
            {
                var subGroup = new ContactGroup(group.Key.ToString());
                subGroup.AddRange(group.ToArray());

                superGroup.Add(subGroup);
            }

            return superGroup;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
