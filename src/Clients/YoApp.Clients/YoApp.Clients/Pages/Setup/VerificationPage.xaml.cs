using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;
using static YoApp.Clients.Core.MathHelper;

namespace YoApp.Clients.Pages.Setup
{
    public partial class VerificationPage : ContentPage, IPageService
    {
        private const string AnimationEnter = nameof(AnimationEnter);
        private bool _hasFinishedAnimation;

        public VerificationPage(string phoneNumber)
        {
            InitializeComponent();

            BindingContext = App.Container.Resolve<VerificationViewModel>(
                new NamedParameter("phoneNumber", phoneNumber),
                new TypedParameter(typeof(IPageService), this));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //PresentPage();
        }

        private void PresentPage()
        {
            if (_hasFinishedAnimation)
                return;

            var animationController = new Animation();

            var fadeAnimation = new Animation(v => GridContainer.Opacity = v, 0, 1, Easing.CubicIn);

            var startOffset = GridContainer.Height * 0.10;
            var containerMove = new Animation(
                v => GridContainer.TranslationY = Lerp(startOffset, 0, v),
                0, 1,
                Easing.CubicInOut);

            animationController.Add(0, 0.65, fadeAnimation);
            animationController.Add(0, 1, containerMove);

            animationController.Commit(this, AnimationEnter,
                16, 500, null,
                async (d, b) =>
                {
                    await Task.Delay(500);
                    CodeEntry.Focus();
                    _hasFinishedAnimation = true;
                });
        }
    }
}
