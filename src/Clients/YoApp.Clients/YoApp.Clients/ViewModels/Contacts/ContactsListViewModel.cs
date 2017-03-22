namespace YoApp.Clients.ViewModels.Contacts
{
    //public class ContactsListViewModel : INotifyPropertyChanged
    //{
    //    public List<ContactGroup> ContactGroups
    //    {
    //        get { return _contactGroups; }
    //        private set
    //        {
    //            _contactGroups = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    public Command TappedCommand { get; private set; }
    //    public Command RefreshCommand { get; private set; }

    //    private List<ContactGroup> _contactGroups;
    //    private readonly IPageService _pageService;
    //    private bool _isFetching;
    //    private bool _canOpenContact = true;
    //    private readonly IContactsManager _contactsManager;

    //    public ContactsListViewModel(IPageService pageService)
    //    {
    //        _pageService = pageService;
    //        _contactsManager = App.Resolver.Resolve<IContactsManager>();

    //        TappedCommand = new Command(async
    //            (o) => await OpenContactDetails(o),
    //            (o) => _canOpenContact);

    //        RefreshCommand = new Command(async
    //            (o) => await RefreshContacts(),
    //            (o) => !_isFetching);

    //        MessagingCenter.Subscribe<App, LifecycleEventArgs>(this, MessagingEvents.OnLifecycleChanged,
    //            (o, a) =>
    //            {
    //                if (a.State == Lifecycle.Resume && !_isFetching)
    //                    RefreshCommand.Execute(o);
    //            });
    //    }

    //    private async Task OpenContactDetails(object selection)
    //    {
    //        var item = selection as Contact;

    //        _canOpenContact = false;
    //        await _pageService.Navigation.PushAsync(new ContactDetailPage(item));
    //        _canOpenContact = true;
    //    }

    //    private async Task RefreshContacts()
    //    {
    //        if (_isFetching) return;

    //        _isFetching = true;
    //        RefreshCommand.ChangeCanExecute();

    //        if (await _contactsManager.LoadContactsAsync())
    //        {
    //            if (Helpers.Settings.InitSetupDone)
    //            {
    //                _contactsManager.MatchFriends();
    //                await _contactsManager.DiscoverFriendsAsync();
    //            }

    //            ContactGroups = ContactFactory.CreateContactGroups(_contactsManager.Contacts);
    //        }

    //        _isFetching = false;
    //        RefreshCommand.ChangeCanExecute();
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}
}
