using System;
using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Setup;
using static YoApp.Clients.Core.MathHelper;

namespace YoApp.Clients.Pages.Setup
{
    public partial class WelcomePage : ContentPage, IPageService
    {
        private bool _sizeAllocated;
        private const string AnimationEnter = nameof(AnimationEnter);
        private const string AnimationExit = nameof(AnimationExit);

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

            var animation = GetAnimationStart();
            animation.Commit(this, AnimationEnter, 16, 1000);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private Animation GetAnimationStart()
        {
            var animationController = new Animation();

            var layoutHeight = GridLayout.Height;
            SlideSecond.TranslationY = layoutHeight;
            SlideThird.TranslationY = layoutHeight;
            SlideSecond.Scale = 1.5;
            SlideSecond.AnchorX = 0.20;
            SlideSecond.AnchorY = 0.20;
            SlideThird.Scale = 1.5;
            SlideThird.AnchorX = 0.20;
            SlideThird.AnchorY = 0.20;

            var slideInSecond = new Animation(v => SlideSecond.TranslationY = Lerp(layoutHeight, 0, v), 0, 0.55, Easing.SinIn);
            var rotateSecond = new Animation(v => SlideSecond.Rotation = Lerp(0, -25, v), 0, 1, Easing.SinIn);
            var slideInThird = new Animation(v => SlideThird.TranslationY = Lerp(layoutHeight, 0, v), 0, 0.10, Easing.SinIn);
            var rotateThird = new Animation(v => SlideThird.Rotation = Lerp(0, -15, v), 0, 1 , Easing.SinIn);

            animationController.Add(0, 0.85, slideInSecond);
            animationController.Add(0, 0.87, rotateSecond);
            animationController.Add(0.25, 0.98, slideInThird);
            animationController.Add(0.25, 1, rotateThird);

            return animationController;
        }
    }
}
