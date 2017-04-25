using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages;

namespace YoApp.Clients.ViewModels.Setup
{
    public class CompleteViewModel
    {
        public string Topic => $"Welcome aboard {_appUserManager?.User.Nickname}!";
        public Command FinishCommand { get; }

        private readonly IAppUserManager _appUserManager;

        public CompleteViewModel(IAppUserManager appUserManager, IPageService pageService)
        {
            _appUserManager = appUserManager;

            FinishCommand = new Command(
                ()=> Application.Current.MainPage = new NavigationPage(new MainPage()));
        }
    }
}
