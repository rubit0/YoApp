using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;
using static YoApp.Clients.Core.MathHelper;

namespace YoApp.Clients.Pages.Setup
{
    public partial class WelcomePage : ContentPage, IPageService
    {
        private bool _presentedEnterAnimation;
        private const string AnimationEnter = nameof(AnimationEnter);

        public WelcomePage()
        {
            InitializeComponent();

            BindingContext = App.Container.Resolve<WelcomeViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (!_presentedEnterAnimation)
                StartPagePresentation();
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

        private void StartPagePresentation(uint duration = 2000)
        {
            var animationController = new Animation();
            var captionsAnimationCotnroller =new Animation();

            ButtonContinue.Opacity = 0;
            var layoutHeight = GridLayout.Height;

            SlideSecond.TranslationY = -layoutHeight;
            SlideSecond.Opacity = 0;
            SlideSecond.Scale = 1.5;
            SlideSecond.AnchorY = 1;

            SlideThird.TranslationY = layoutHeight;
            SlideThird.Opacity = 0;
            SlideThird.Scale = 1.5;
            SlideThird.AnchorY = 0;

            SplashIcon.Opacity = 0;
            Overlay.Opacity = 0;
            SplashIcon.RotationY = -90;
            IconsContainer.Spacing = -0;
            IconsContainer.Opacity = 0;

            var iconsAnimation = new Animation(v => IconsContainer.Spacing = v, 0, 30, Easing.CubicOut);
            var iconsFade = new Animation(v => IconsContainer.Opacity = v);

            var captions = CaptionsContainer.Children;
            for (var i = 0; i < captions.Count; i++)
            {
                captions[i].Opacity = 0;
                var iterator = i;
                var fader = new Animation(v => captions[iterator].Opacity = v, 0, 1, Easing.CubicIn);
                double fraction = 1f / captions.Count;
                var begin = fraction * i;
                var end = fraction * (i + 1);
                captionsAnimationCotnroller.Add(begin, end, fader);
            }

            var slideInSecond = new Animation(
                v => SlideSecond.TranslationY = Lerp(-layoutHeight, layoutHeight, v), 
                0, 0.13, 
                Easing.CubicInOut);
            var slideInThird = new Animation(
                v => SlideThird.TranslationY = Lerp(layoutHeight, -layoutHeight, v), 
                0, 0.12, 
                Easing.CubicInOut);
            var rotateSecond = new Animation(
                v => SlideSecond.Rotation = Lerp(0, -25, v),
                0, 1,
                Easing.CubicInOut);
            var rotateThird = new Animation(
                v => SlideThird.Rotation = Lerp(0, -15, v),
                0, 1,
                Easing.CubicInOut);
            var rotateIcon = new Animation(
                v => SplashIcon.RotationY = Lerp(-90, 0, v),
                0, 1,
                Easing.SpringOut);
            var fadeInContinueButton = new Animation(
                v => ButtonContinue.Opacity = v,
                0, 1,
                Easing.SinIn);

            var fadeInFirst = new Animation(v => SlideFirst.Opacity = v);
            var fadeInSecond = new Animation(v => SlideSecond.Opacity = v);
            var fadeInThird = new Animation(v => SlideThird.Opacity = v);
            var fadeInIcon = new Animation(v => SplashIcon.Opacity = v);
            var fadeInOverlay = new Animation(v => Overlay.Opacity = v, 0, 0.6);

            animationController.Add(0, 0.5, fadeInFirst);
            animationController.Add(0.3, 0.7, fadeInIcon);
            animationController.Add(0.5, 1, rotateIcon);
            animationController.Add(0, 0.5, fadeInOverlay);
            animationController.Add(0.5, 1, iconsAnimation);
            animationController.Add(0.4, 0.6, iconsFade);

            animationController.Add(0.2, 0.8, slideInSecond);
            animationController.Add(0.1, 0.8, rotateSecond);
            animationController.Add(0, 0.5, fadeInSecond);

            animationController.Add(0.2, 0.8, slideInThird);
            animationController.Add(0.1, 0.8, rotateThird);
            animationController.Add(0, 0.5, fadeInThird);
            animationController.Add(0.8, 1, fadeInContinueButton);

            _presentedEnterAnimation = true;

            animationController.Commit(this, AnimationEnter, 16, duration, 
                finished: (d, b) => captionsAnimationCotnroller.Commit(this, AnimationEnter, 16, (uint)captions.Count * 500));
        }
    }
}
