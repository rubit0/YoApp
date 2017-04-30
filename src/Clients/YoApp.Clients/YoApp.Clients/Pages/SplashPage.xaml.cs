using System;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.StateMachine.States;

namespace YoApp.Clients.Pages
{
    public partial class SplashPage : ContentPage
    {
        private const string StartAnimation = nameof(StartAnimation);
        private const string ExitAnimation = nameof(ExitAnimation);

        public SplashPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<LifeCycleState, Page>(
                this, 
                MessagingEvents.AppLoadFinished, 
                FadeOut);
        }

        protected override void OnAppearing()
        {
            BackDrop.Opacity = 0;
            ActivityIndicator.Opacity = 0;

            if (Device.RuntimePlatform == Device.Android)
                Logo.RotationY = -90;

            PresentEntryAnimation(1250);
        }

        protected override void OnDisappearing()
        {
            this.AbortAnimation(StartAnimation);
            this.AbortAnimation(ExitAnimation);
        }

        private void FadeOut(object sender, Page target)
        {
            this.CancelAnimation();
            Logo.RotationY = 0;
            Logo.Opacity = 1;

            PresentExitAnimation(850, target);
        }

        private void PresentEntryAnimation(uint duration)
        {
            var animationController = new Animation();
            var fadeIconsBackDrop = new Animation(v => BackDrop.Opacity = v, 0, 0.5, Easing.CubicIn);
            var fadeActivityIndicator = new Animation(v => ActivityIndicator.Opacity = v);

            animationController.Add(0.55, 1, fadeIconsBackDrop);
            animationController.Add(0.85, 1, fadeActivityIndicator);

            if (Device.RuntimePlatform == Device.Android)
            {
                var startColor = new Color(1);
                var targetColor = new Color(0.196, 0.639, 0.839);
                var tintBackground = new Animation(v => Container.LerpBackgroundColor(startColor, targetColor, v));
                var fadeLogo = new Animation(v => Logo.Opacity = v);
                var rotateLogo = new Animation(v => Logo.RotationY = v, -90, 0, Easing.BounceIn);

                animationController.Add(0, 0.65, tintBackground);
                animationController.Add(0.35, 0.8, fadeLogo);
                animationController.Add(0.5, 1, rotateLogo);
            }

            animationController.Commit(this, StartAnimation, 16, duration);
        }

        private void PresentExitAnimation(uint duration, Page nextPage)
        {
            var animationController = new Animation();

            var startColor = Container.BackgroundColor;
            var targetColor = new Color(1);
            var tintBackground = new Animation(v => Container.LerpBackgroundColor(startColor, targetColor, v));
            var fadeIconsBackDrop = new Animation(v => BackDrop.Opacity = v, BackDrop.Opacity, 0, Easing.CubicOut);
            var fadeLogo = new Animation(v => Logo.Opacity = v, 1, 0);
            var fadeActivityIndicator = new Animation(v => ActivityIndicator.Opacity = v, 1, 0);
            var fadeCircle = new Animation(v => Circle.Opacity = v);
            var scaleCircle = new Animation(v => Circle.Scale = v, 0, 10);
            var rotateLogo = new Animation(v => Logo.RotationY = v, Logo.RotationY, -90, Easing.BounceIn);

            animationController.Add(0, 0.65, tintBackground);
            animationController.Add(0, 0.55, fadeIconsBackDrop);
            animationController.Add(0.2, 0.75, fadeLogo);
            animationController.Add(0, 0.5, fadeActivityIndicator);
            animationController.Add(0, 0.45, fadeCircle);
            animationController.Add(0, 0.75, scaleCircle);
            animationController.Add(0, 0.5, rotateLogo);

            animationController.Commit(this, 
                ExitAnimation, 16, duration, Easing.SinOut, 
                (current, done) => App.Current.MainPage = nextPage);
        }
    }
}
