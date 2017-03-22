using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Modals;
using YoApp.Clients.Pages.Setup;
using YoApp.Clients.Services;

namespace YoApp.Clients.ViewModels.Setup
{
    public class VerificationViewModel
    {
        public string PhoneNumber { get; }
        public string VerificationCode { get; set; }
        public Command VerifyCommand { get; }

        private readonly IVerificationManager _verificationManager;
        private readonly IPageService _pageService;
        private bool _canVerify = true;

        public VerificationViewModel(string phoneNumber, IPageService pageService)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber), "You must provide a phone number");

            PhoneNumber = phoneNumber;
            _pageService = pageService;
            _verificationManager = App.Resolver.Resolve<IVerificationManager>();

            VerifyCommand = new Command(async () => await StartVerification(),
                () => _canVerify
                && CrossConnectivity.Current.IsConnected);

            CrossConnectivity.Current.ConnectivityChanged +=
                (sender, args) => VerifyCommand.ChangeCanExecute();
        }

        private async Task StartVerification()
        {
            _canVerify = false;
            VerifyCommand.ChangeCanExecute();
            var password = AuthenticationService.GeneratePassword();

            await _pageService.Navigation.PushModalAsync(new LoadingModalPage("Verifying security code..."));

            var response = await _verificationManager.ResolveVerificationCodeAsync(VerificationCode, PhoneNumber, password);

            await _pageService.Navigation.PopModalAsync();

            if (!response)
            {
                _canVerify = true;
                VerifyCommand.ChangeCanExecute();

                await _pageService.DisplayAlert("Code Error",
                    "The code does not match, please try again.",
                    "Ok");
            }
            else
            {
                var userManager = App.Resolver.Resolve<IAppUserManager>();
                App.Settings.SetupFinished = true;
                await userManager.InitUserAsync(PhoneNumber);
                await AuthenticationService.RequestToken(PhoneNumber, password);

                await Task.Run(() => MessagingCenter.Send(this, MessagingEvents.UserCreated));

                await _pageService.Navigation.PushAsync(new ProfilePage());
            }
        }
    }
}
