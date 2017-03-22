using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
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
        private readonly IPageService _pageService;
        private readonly IContactsManager _contactsManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContactsSelectionViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _contactsManager = App.Resolver.Resolve<IContactsManager>();

            ContactGroups = _contactsManager.BuildContactGroup();

            TappedCommand = new Command(async
                (o) => await _pageService.DisplayAlert("Clicked", o.ToString(), "Ok"));

            _contactsManager.PropertyChanged += (sender, args) =>
            {
                if (string.CompareOrdinal(args.PropertyName, nameof(_contactsManager.Contacts)) == 0)
                    ContactGroups = _contactsManager.BuildContactGroup();
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
