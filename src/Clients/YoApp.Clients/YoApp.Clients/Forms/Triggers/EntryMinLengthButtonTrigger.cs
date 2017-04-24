using Xamarin.Forms;

namespace YoApp.Clients.Forms.Triggers
{
    public class EntryMinLengthButtonTrigger : TriggerAction<Entry>
    {
        public Button Target { get; set; }
        public int MinLength { get; set; }

        protected override void Invoke(Entry sender)
        {
            if(Target == null)
                return;
            
            Target.IsEnabled = (sender.Text.Length >= MinLength);
        }
    }
}