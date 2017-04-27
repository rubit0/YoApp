using Xamarin.Forms;

namespace YoApp.Clients.Forms.Triggers
{
    public class EntryMinLengthTrigger : TriggerAction<Entry>
    {
        public VisualElement ToggleTarget { get; set; }
        public int MinLength { get; set; }

        protected override void Invoke(Entry sender)
        {
            if (ToggleTarget == null)
                return;

            ToggleTarget.IsEnabled = (sender.Text.Length >= MinLength);
        }
    }
}