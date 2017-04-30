using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Core.EventArgs;
using YoApp.Clients.Forms;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Setup;
using YoApp.Clients.Services;

namespace YoApp.Clients.ViewModels.Setup
{
    public class VerificationViewModel
    {
        public string Title => $"Verify +{PhoneNumber}";
        public string PhoneNumber { get; }
        public string VerificationCode { get; set; }
        public Command VerifyCommand { get; }

        private readonly IVerificationManager _verificationManager;
        private readonly IAppUserManager _appUserManager;
        private readonly IPageService _pageService;
        private readonly IUserDialogs _userDialogs;
        private bool _canVerify = true;

        public VerificationViewModel(string phoneNumber, IPageService pageService, IVerificationManager verificationManager, 
            IAppUserManager appUserManager, IUserDialogs userDialogs)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber), "You must provide a phone number");

            PhoneNumber = phoneNumber;
            _pageService = pageService;
            _verificationManager = verificationManager;
            _appUserManager = appUserManager;
            _userDialogs = userDialogs;

            VerifyCommand = new Command(async () => await StartVerification(),
                () => _canVerify
                && CrossConnectivity.Current.IsConnected);

            CrossConnectivity.Current.ConnectivityChanged +=
                (sender, args) => VerifyCommand.ChangeCanExecute();
        }

        private async Task StartVerification()
        {
            _canVerify = false;
            _userDialogs.ShowLoading("Verifying security code.");

            VerifyCommand.ChangeCanExecute();
            var password = AuthenticationService.GeneratePassword();
            var response = await _verificationManager.ResolveVerificationCodeAsync(VerificationCode, PhoneNumber, password);

            _userDialogs.HideLoading();

            if (!response)
            {
                _canVerify = true;
                VerifyCommand.ChangeCanExecute();

                _userDialogs.ShowError("\nThe verification code doesn't match.", 2500);
            }
            else
            {
                App.Settings.SetupFinished = true;
                _appUserManager.InitUser(PhoneNumber);

                await _appUserManager.PersistUser();
                await AuthenticationService.RequestToken(PhoneNumber, password);

                MessagingCenter.Send(this, 
                    MessagingEvents.LifecycleChanged, new 
                    LifecycleEventArgs(Lifecycle.SetupCompleted));

                await _pageService.Navigation.PushAsync(new ProfilePage());
            }
        }
    }
}
