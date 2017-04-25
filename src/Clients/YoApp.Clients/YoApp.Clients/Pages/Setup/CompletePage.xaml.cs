using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class CompletePage : ContentPage, IPageService
    {
        public CompletePage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = App.Container.Resolve<CompleteViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
