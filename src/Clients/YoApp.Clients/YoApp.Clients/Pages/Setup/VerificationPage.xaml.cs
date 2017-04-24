using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class VerificationPage : ContentPage, IPageService
    {
        public VerificationPage(string phoneNumber)
        {
            InitializeComponent();
            CodeEntry.Focus();

            BindingContext = App.Container.Resolve<VerificationViewModel>(
                new NamedParameter("phoneNumber", phoneNumber),
                new TypedParameter(typeof(IPageService), this));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CodeEntry.Focus();
        }
    }
}
