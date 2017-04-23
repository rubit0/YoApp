using Plugin.Connectivity;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Core.Extensions;
using YoApp.Clients.Forms;
using YoApp.Clients.Pages.Modals;
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

        public WelcomeViewModel(IPageService pageService)
        {
            _pageService = pageService;

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

            await _pageService.Navigation.PushModalAsync(new LoadingModalPage("Please wait, connecting to service."));

            if (!CrossConnectivity.Current.IsConnected
                || !await CrossConnectivity.Current.IsServiceOnlineAsync())
            {
                await _pageService.Navigation.PopModalAsync();
                IsConnecting = false;
                ConnectCommand.ChangeCanExecute();

                return;
            }

            await _pageService.Navigation.PopModalAsync();

            await _pageService.Navigation.PushAsync(new EnterNumberPage());
        }
    }
}
