using Plugin.Connectivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Setup;

namespace YoApp.Clients.ViewModels.Setup
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NickName
        {
            get { return App.Settings.User.Nickname; }
            set { App.Settings.User.Nickname = value; }
        }

        public string StatusMessage
        {
            get { return App.Settings.User.Status; }
            set { App.Settings.User.Status = value; }
        }

        public ICommand SubmitCommand { get; }
        public bool CanSubmit { get; private set; } = true;

        private readonly IPageService _pageService;
        private readonly IAppUserManager _appUserManager;

        public ProfileViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _appUserManager = App.Resolver.Resolve<IAppUserManager>();

            SubmitCommand = new Command(async () => await UpdateAndPersistUser(),
                () => CanSubmit);
        }

        private async Task UpdateAndPersistUser()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await _pageService.Navigation.PushAsync(new CompletePage());
                return;
            }

            CanSubmit = false;

            await _appUserManager.SyncUpAsync();
            await _pageService.Navigation.PushAsync(new CompletePage());
        }

        public async Task FetchUserFromServer()
        {
            if (!CrossConnectivity.Current.IsConnected
                || !await _appUserManager.SyncDownAsync())
                return;

            OnPropertyChanged(nameof(NickName));
            OnPropertyChanged(nameof(StatusMessage));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
