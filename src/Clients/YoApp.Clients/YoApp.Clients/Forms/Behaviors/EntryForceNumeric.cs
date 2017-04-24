using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace YoApp.Clients.Forms.Behaviors
{
    public class EntryForceNumeric : Behavior<Entry>
    {
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

        private void OnTextChanged(object sender, TextChangedEventArgs eventArgs)
        {
            _entry.Text = Regex.Replace(eventArgs.NewTextValue, @"\D", string.Empty);
        }
    }
}
