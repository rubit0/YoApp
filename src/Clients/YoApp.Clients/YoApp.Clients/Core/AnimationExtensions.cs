using Xamarin.Forms;

namespace YoApp.Clients.Core
{
    public static class AnimationExtensions
    {
        public static void LerpBackgroundColor(this VisualElement target, Color fromColor, Color toColor, double current)
        {
            var red = fromColor.R + (toColor.R - fromColor.R) * current;
            var green = fromColor.G + (toColor.G - fromColor.G) * current;
            var blue = fromColor.B + (toColor.B - fromColor.B) * current;
            var alpha = fromColor.A + (toColor.A - fromColor.A) * current;

            target.BackgroundColor = new Color(red, green, blue, alpha);
        }
    }
}
