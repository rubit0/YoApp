using Plugin.Connectivity;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using YoApp.Clients.Core.Extensions;
using YoApp.Clients.Forms;
using YoApp.Clients.Pages.Setup;

namespace YoApp.Clients.ViewModels.Setup
{
    public class WelcomeViewModel
    {
        public Command ConnectCommand { get; }
        public Command PresentTermsCommand { get; }

        private bool _isConnecting;
        public bool IsConnecting
        {
            get => _isConnecting;
            set
            {
                _isConnecting = value;
                ConnectCommand.ChangeCanExecute();
            }
        }

        private readonly IPageService _pageService;
        private readonly IUserDialogs _userDialogs;

        public WelcomeViewModel(IPageService pageService, IUserDialogs userDialogs)
        {
            _pageService = pageService;
            _userDialogs = userDialogs;

            ConnectCommand = new Command(async () => await TestServiceConnection(),
                () => !IsConnecting && CrossConnectivity.Current.IsConnected);

            PresentTermsCommand = new Command(async () => await pageService.DisplayAlert("YoApp Terms", "Lorem ipsum.", "Ok"));

            CrossConnectivity.Current.ConnectivityChanged +=
                (sender, args) => ConnectCommand.ChangeCanExecute();
        }

        private async Task TestServiceConnection()
        {
            IsConnecting = true;
            ConnectCommand.ChangeCanExecute();
            var loadDialog = _userDialogs.Loading("Connecting to service");
            loadDialog.Show();

            if (!CrossConnectivity.Current.IsConnected
                || !await CrossConnectivity.Current.IsServiceOnlineAsync())
            {
                loadDialog.Hide();
                loadDialog.Dispose();

                await _pageService.Navigation.PopModalAsync();
                IsConnecting = false;
                ConnectCommand.ChangeCanExecute();

                return;
            }

            loadDialog.Hide();
            loadDialog.Dispose();

            await _pageService.Navigation.PushAsync(new EnterNumberPage());
        }
    }
}
