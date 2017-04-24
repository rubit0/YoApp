using System.Threading.Tasks;
using Xamarin.Forms;

namespace YoApp.Clients.Forms.Behaviors
{
    public class TimedSelfDisable : Behavior<VisualElement>
    {
        public int Delay { get; set; } = 10;

        protected override async void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);

            await Task.Delay(Delay);

            if (bindable != null)
                bindable.IsEnabled = false;
        }
    }
}