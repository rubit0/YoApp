using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Pages.Setup
{
    public partial class WelcomePage : ContentPage, IPageService
    {
        private bool _sizeAllocated;

        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<WelcomeViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.RuntimePlatform == Device.Android)
            {
                _sizeAllocated = true;
                OnAppearing(); 
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.Android)
            {
                if (!_sizeAllocated)
                    return; 
            }

            var height = SlideFirst.Height;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
