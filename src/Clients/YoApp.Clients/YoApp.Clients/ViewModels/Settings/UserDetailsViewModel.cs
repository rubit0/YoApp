using Plugin.Connectivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;
using YoApp.Clients.Manager;

namespace YoApp.Clients.ViewModels.Settings
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _nickName;
        public string Nickname
        {
            get => _nickName;
            set
            {
                _nickName = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateCommand { get; }
        private readonly IAppUserManager _appUserManager;
        private readonly IUserDialogs _userDialogs;

        public UserDetailsViewModel(IAppUserManager appUserManager, IUserDialogs userDialogs)
        {
            _appUserManager = appUserManager;
            _userDialogs = userDialogs;

            _nickName = _appUserManager.User.Nickname;
            _statusMessage = _appUserManager.User.Status;

            UpdateCommand = new Command(async () => await UpdateUser(),
                () => CrossConnectivity.Current.IsConnected);
        }

        public async Task UpdateUser()
        {
            var loadDialog = _userDialogs.Loading("Updating.");
            loadDialog.Show();

            var oldNick = _nickName;
            var oldStatus = _statusMessage;
            _appUserManager.User.Nickname = _nickName;
            _appUserManager.User.Status = _statusMessage;

            var result = await _appUserManager.SyncUpAsync();
            loadDialog.Hide();
            loadDialog.Dispose();

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
