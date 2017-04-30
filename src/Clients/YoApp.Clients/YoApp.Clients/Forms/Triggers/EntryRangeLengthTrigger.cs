using Xamarin.Forms;

namespace YoApp.Clients.Forms.Triggers
{
    public class EntryRangeLengthTrigger : TriggerAction<Entry>
    {
        public VisualElement ToggleTarget { get; set; }

        public int MinLength { get; set; } = 0;
        public int MaxLength { get; set; } = 5;

        protected override void Invoke(Entry sender)
        {
            if (ToggleTarget == null)
                return;

            ToggleTarget.IsEnabled = sender.Text.Length >= MinLength 
                                     && sender.Text.Length <= MaxLength;
        }
    }
}