using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class WelcomePage : ContentPage, IPageService
    {
        public WelcomePage()
        {
            InitializeComponent();
            var viewModel = new WelcomeViewModel(this);
            BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
