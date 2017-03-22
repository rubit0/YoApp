using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class ProfilePage : ContentPage, IPageService
    {
        private ProfileViewModel _viewModel;

        public ProfilePage()
        {
            InitializeComponent();

            _viewModel = new ProfileViewModel(this);
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
