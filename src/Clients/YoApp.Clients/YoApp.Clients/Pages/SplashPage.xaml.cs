using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YoApp.Clients.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            Logo.RotationY = -90;
            Logo.Opacity = 0;

            var targetColor = new Color(0.196, 0.639, 0.839);
            Container.Animate("lerpColor", 
                time =>
                {
                    Container.BackgroundColor = LerpRGBColor(Container.BackgroundColor, targetColor, time);
                }, 
                0, 1, length: 1250, easing: Easing.BounceIn);

            await Task.WhenAll(
                Logo.FadeTo(1, 350),
                Logo.RotateYTo(0, 550, Easing.BounceIn)
                );
        }

        private Color LerpRGBColor(Color source, Color target, double current)
        {
            var red = source.R + (target.R - source.R) * current;
            var green = source.G + (target.G - source.G) * current;
            var blue = source.B + (target.B - source.B) * current;
            var alpha = source.A + (target.A - source.A) * current;

            return new Color(red, green, blue, alpha);
        }
    }
}
