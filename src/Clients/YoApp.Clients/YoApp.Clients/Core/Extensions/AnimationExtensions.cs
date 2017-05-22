using Xamarin.Forms;

namespace YoApp.Clients.Core.Extensions
{
    public static class AnimationExtensions
    {
        /// <summary>
        /// Interpolate a Background color.
        /// </summary>
        /// <param name="target">Target on which the color will be changed.</param>
        /// <param name="fromColor">Start color on the interpolation span.</param>
        /// <param name="toColor">End color on the interpolation span.</param>
        /// <param name="current">Current interpoliation as normalized value.</param>
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
