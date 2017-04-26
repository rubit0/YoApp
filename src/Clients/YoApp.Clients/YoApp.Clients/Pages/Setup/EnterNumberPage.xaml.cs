using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;
using static YoApp.Clients.Core.MathHelper;

namespace YoApp.Clients.Pages.Setup
{
    public partial class EnterNumberPage : ContentPage, IPageService
    {
        private const string AnimationEnter = nameof(AnimationEnter);

        public EnterNumberPage()
        {
            InitializeComponent();
            GridContainer.Opacity = 0;

            NavigationPage.SetHasBackButton(this, false);
            SubmitButton.IsEnabled = false;

            BindingContext = App.Container.Resolve<EnterNumberViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }

        protected override void OnAppearing()
        {
            //NumberEntry.Focus();
            PresentPage();
        }

        protected override void OnDisappearing()
        {
            this.CancelAnimation();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void PresentPage()
        {
            var animationController = new Animation();

            var fadeAnimation = new Animation(v => GridContainer.Opacity = v, 0, 1, Easing.CubicIn);

            var startOffset = GridContainer.Height * 0.10;
            var containerMove = new Animation(
                v => GridContainer.TranslationY = Lerp(startOffset, 0, v), 
                0, 1, 
                Easing.CubicInOut);

            animationController.Add(0 , 0.65, fadeAnimation);
            animationController.Add(0, 1, containerMove);

            animationController.Commit(this, AnimationEnter, 
                16, 500, null, 
                async (d, b) => 
                {
                    await Task.Delay(500);
                    NumberEntry.Focus();
                });

        }
    }
}
