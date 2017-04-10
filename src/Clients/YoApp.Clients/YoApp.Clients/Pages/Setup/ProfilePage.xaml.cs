using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class ProfilePage : ContentPage, IPageService
    {
        private readonly ProfileViewModel _viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            _viewModel = App.Container.Resolve<ProfileViewModel>(
                new TypedParameter(typeof(IPageService), this));

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            if (_viewModel != null)
                await _viewModel.FetchUserFromServer();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
