using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.Manager;
using YoApp.Clients.ViewModels.ListViewGroups;

namespace YoApp.Clients.ViewModels.Settings
{
    public class ContactsSelectionViewModel : INotifyPropertyChanged
    {
        public List<ContactGroup> ContactGroups
        {
            get { return _contactGroups; }
            private set
            {
                _contactGroups = value;
                OnPropertyChanged();
            }
        }

        public Command TappedCommand { get; private set; }

        private List<ContactGroup> _contactGroups;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContactsSelectionViewModel(IPageService pageService, IContactsManager contactsManager)
        {
            var contactsManager1 = contactsManager;
            ContactGroups = contactsManager1.BuildContactGroup();

            TappedCommand = new Command(async
                (o) => await pageService.DisplayAlert("Clicked", o.ToString(), "Ok"));

            contactsManager1.PropertyChanged += (sender, args) =>
            {
                if (string.CompareOrdinal(args.PropertyName, nameof(contactsManager1.Contacts)) == 0)
                    ContactGroups = contactsManager1.BuildContactGroup();
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
