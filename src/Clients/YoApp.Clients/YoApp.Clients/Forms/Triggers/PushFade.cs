using System.Threading.Tasks;
using Xamarin.Forms;
using static YoApp.Clients.Core.MathHelper;

namespace YoApp.Clients.Forms.Triggers
{
    public class PushFade : TriggerAction<VisualElement>
    {
        public enum PushDirection
        {
          Top,
          Right,
          Bottom,
          Left
        };

        /// <summary>
        /// Direction this animation will push towards.
        /// </summary>
        public PushDirection Direction { get; set; } = PushDirection.Right;

        /// <summary>
        /// Normalized position offset to start the animation from.
        /// A Positive value starts from the bottom and vice versa.
        /// </summary>
        public double Offset { get; set; } = 0.075;

        /// <summary>
        /// Animation duration in milliseconds.
        /// </summary>
        public int Duration { get; set; } = 1000;

        /// <summary>
        /// Play this animation only once.
        /// </summary>
        public bool PlayOnce { get; set; } = true;

        /// <summary>
        /// Optional Element to focus at after animation is completed.
        /// </summary>
        public VisualElement FocusElement { get; set; }

        /// <summary>
        /// Optionaly set a different target for this animation.
        /// </summary>
        public VisualElement Target { get; set; }

        private bool _hasPlayed;

        protected override void Invoke(VisualElement sender)
        {
            if (PlayOnce && _hasPlayed)
                return;

            if (Target == null)
                Target = sender;

            Target.Opacity = 0;
            Target.AnchorX = 0.5;
            Target.AnchorY = 0.5;

            var animationController = new Animation();

            switch (Direction)
            {
                case PushDirection.Top:
                    VerticalAnimation(animationController);
                    break;
                case PushDirection.Right:
                    HorizontalAnimation(animationController);
                    break;
                case PushDirection.Bottom:
                    VerticalAnimation(animationController, true);
                    break;
                case PushDirection.Left:
                    HorizontalAnimation(animationController, true);
                    break;
                default:
                    return;
            }

            var fadeAnimation = new Animation( v => Target.Opacity = v, easing: Easing.CubicIn);
            animationController.Add(0.25, 0.65, fadeAnimation);

            animationController.Commit(Target, nameof(PushFade),
                16,
                (uint)Duration,
                finished: async (d, b) =>
                {
                    if (FocusElement == null)
                        return;

                    _hasPlayed = true;
                    await Task.Delay(500);
                    FocusElement.Focus();
                });
        }

        private void VerticalAnimation(Animation animationController, bool negative = false)
        {
            var rotation = (negative) ? -30 : 30;
            Target.RotationX = rotation;

            var rotationAnimation = new Animation(v => Target.RotationX = v, rotation, 0, Easing.CubicInOut);
            var startOffset = Target.Height * ((negative) ? Offset : -Offset);
            var containerMove = new Animation(
                v => Target.TranslationY = Lerp(startOffset, 0, v),
                0, 1,
                Easing.CubicInOut);

            animationController.Add(0, 0.65, rotationAnimation);
            animationController.Add(0, 1, containerMove);
        }

        private void HorizontalAnimation(Animation animationController, bool negative = false)
        {
            var rotation = (negative) ? -30 : 30;
            Target.RotationY = rotation;

            var rotationAnimation = new Animation(v => Target.RotationY = v, rotation, 0, Easing.CubicInOut);
            var startOffset = Target.Width * ((negative) ? Offset : -Offset);
            var containerMove = new Animation(
                v => Target.TranslationX = Lerp(startOffset, 0, v),
                0, 1,
                Easing.CubicInOut);

            animationController.Add(0, 0.65, rotationAnimation);
            animationController.Add(0, 1, containerMove);
        }
    }
}