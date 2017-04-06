using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class EnterNumberPage : ContentPage, IPageService
    {
        public EnterNumberPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<EnterNumberViewModel>(
                new TypedParameter(typeof(IPageService), this));

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
