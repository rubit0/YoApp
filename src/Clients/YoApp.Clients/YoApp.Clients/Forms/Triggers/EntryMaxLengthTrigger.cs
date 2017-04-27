using Xamarin.Forms;

namespace YoApp.Clients.Forms.Triggers
{
    public class EntryMaxLengthTrigger : TriggerAction<Entry>
    {
        public VisualElement ToggleTarget { get; set; }
        public int MaxLength { get; set; }

        protected override void Invoke(Entry sender)
        {
            if (ToggleTarget == null)
                return;

            ToggleTarget.IsEnabled = (sender.Text.Length <= MaxLength);
        }
    }
}
