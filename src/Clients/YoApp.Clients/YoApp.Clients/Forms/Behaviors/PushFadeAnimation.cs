using System.Threading.Tasks;
using Xamarin.Forms;
using static YoApp.Clients.Core.MathHelper;

namespace YoApp.Clients.Forms.Behaviors
{
    /// <summary>
    /// Gently pushes an VisualElement with a quickly fading opacity.
    /// </summary>
    public class PushFadeAnimation : Behavior<VisualElement>
    {
        /// <summary>
        /// Duration of the animation in milliseconds.
        /// </summary>
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Normalized vertical offset to start the animation from.
        /// A Positive value starts from the bottom and vice versa.
        /// </summary>
        public double YOffset { get; set; } = 0.075;

        /// <summary>
        /// Optional Element to focus at after animation is completed.
        /// </summary>
        public VisualElement FocusElement { get; set; }

        private const string AnimationHandle = nameof(PushFadeAnimation) + "Handle";
        private bool _hasAnimated;

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Opacity = 0;

            bindable.SizeChanged += (sender, args) =>
            {
                if(_hasAnimated)
                    return;

                var animationController = new Animation();

                var fadeAnimation = new Animation(
                    v => bindable.Opacity = v,
                    0, 1,
                    Easing.CubicIn);

                var startOffset = bindable.Height * YOffset;
                var containerMove = new Animation(
                    v => bindable.TranslationY = Lerp(startOffset, 0, v),
                    0, 1,
                    Easing.CubicInOut);

                animationController.Add(0.25, 0.65, fadeAnimation);
                animationController.Add(0, 1, containerMove);

                animationController.Commit(bindable, AnimationHandle,
                    16, 
                    (uint)Duration, 
                    finished: async (d, b) =>
                    {
                        if (FocusElement == null)
                            return;

                        await Task.Delay(500);
                        FocusElement.Focus();
                        _hasAnimated = true;
                    });
            };
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            bindable.AbortAnimation(AnimationHandle);
            base.OnDetachingFrom(bindable);
        }
    }
}
