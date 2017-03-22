using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class WaitVerificationPage : ContentPage, IPageService
    {
        public WaitVerificationPage(string phoneNumber)
        {
            InitializeComponent();
            CodeEntry.Focus();

            var viewModel = new VerificationViewModel(phoneNumber, this);
            BindingContext = viewModel;
        }
    }
}
