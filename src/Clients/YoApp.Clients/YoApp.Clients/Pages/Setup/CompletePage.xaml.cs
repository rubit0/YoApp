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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var animationController = new Animation();

            OverlayBackground.Opacity = 0;
            var overlayFade = new Animation(
                v => OverlayBackground.Opacity = v, 
                0, 1, 
                Easing.CubicInOut);

            var gradientRotation = new Animation(v => GradientBackground.Rotation = v, 0, 360);

            animationController.Add(0, 0.075, overlayFade);
            animationController.Add(0, 1, gradientRotation);

            animationController.Commit(this, "Animation", 16, 50000, null, null, () => true);
        }
    }
}
