using Xamarin.Forms;

namespace YoApp.Clients.Forms.Behaviors
{
    public class EntryMaxLength : Behavior<Entry>
    {
        public int MaxLength { get; set; }
        private Entry _entry;

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            _entry = bindable;
            _entry.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            _entry.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (textChangedEventArgs.NewTextValue.Length > MaxLength)
                _entry.Text = textChangedEventArgs.OldTextValue;
        }
    }
}
