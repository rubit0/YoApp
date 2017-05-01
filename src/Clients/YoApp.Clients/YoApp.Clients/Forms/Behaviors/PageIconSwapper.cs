using System;
using Xamarin.Forms;

namespace YoApp.Clients.Forms.Behaviors
{
    public class PageIconSwapper : Behavior<Page>
    {
        public string ImageNormal { get; set; }
        public string ImageFocus { get; set; }
        public bool IgnoreAndroidPlatform { get; set; } = true;

        protected override void OnAttachedTo(Page bindable)
        {
            if(IgnoreAndroidPlatform && Device.RuntimePlatform == Device.Android)
                return;

            if (string.IsNullOrWhiteSpace(ImageNormal))
                ImageNormal = bindable.Icon.File 
                    ?? throw new ArgumentNullException($"You must set {nameof(ImageNormal)}.");

            if (string.IsNullOrWhiteSpace(ImageFocus))
                throw new ArgumentNullException($"You must set {nameof(ImageFocus)}.");

            base.OnAttachedTo(bindable);

            bindable.Appearing += (sender, args) => ((Page)sender).Icon = ImageFocus;
            bindable.Disappearing += (sender, args) => ((Page)sender).Icon = ImageNormal;
        }
    }
}
