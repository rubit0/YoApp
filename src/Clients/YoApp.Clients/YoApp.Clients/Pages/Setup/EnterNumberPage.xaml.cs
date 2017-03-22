using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class EnterNumberPage : ContentPage, IPageService
    {
        private EnterNumberViewModel _viewModel;

        public EnterNumberPage()
        {
            InitializeComponent();
            _viewModel = new EnterNumberViewModel(this);
            BindingContext = _viewModel;
            SubmitButton.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            NumberEntry.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
