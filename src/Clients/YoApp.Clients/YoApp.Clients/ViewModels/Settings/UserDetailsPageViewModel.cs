using Plugin.Connectivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Modals;

namespace YoApp.Clients.ViewModels.Settings
{
    public class UserDetailsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _nickName;
        public string Nickname
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateCommand { get; private set; }
        private readonly IPageService _pageService;
        private readonly IAppUserManager _appUserManager;

        public UserDetailsPageViewModel(IPageService pageService, IAppUserManager appUserManager)
        {
            _appUserManager = appUserManager;
            _nickName = _appUserManager.User.Nickname;
            _statusMessage = _appUserManager.User.Status;
            _pageService = pageService;

            UpdateCommand = new Command(async () => await UpdateUser(),
                () => CrossConnectivity.Current.IsConnected);
        }

        public async Task UpdateUser()
        {
            await _pageService.Navigation.PushModalAsync(new LoadingModalPage("Please wait."));

            var oldNick = _nickName;
            var oldStatus = _statusMessage;
            _appUserManager.User.Nickname = _nickName;
            _appUserManager.User.Status = _statusMessage;

            var result = await _appUserManager.SyncUpAsync();
            await _pageService.Navigation.PopModalAsync();

            if (!result)
            {
                //revert changes
                _appUserManager.User.Nickname = oldNick;
                _appUserManager.User.Status = _statusMessage;
                Nickname = oldNick;
                StatusMessage = oldStatus;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
