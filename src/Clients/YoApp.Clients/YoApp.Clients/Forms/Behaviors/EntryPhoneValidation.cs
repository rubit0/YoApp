using System.Text.RegularExpressions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace YoApp.Clients.Forms.Behaviors
{
    public class EntryPhoneValidation : Behavior<Entry>
    {
        public Button SubmitButton { get; set; }
        private Entry _entry;
        private readonly int _phoneNumberMaxLength;
        private readonly IConnectivity _connectivity;

        public EntryPhoneValidation()
        {
            //Calling code can be 4 digits long
            _phoneNumberMaxLength = App.Settings.Conventions.PhoneNumberMaxLength - 4;
            _connectivity = CrossConnectivity.Current;

            _connectivity.ConnectivityChanged += (sender, args) =>
            {
                EntryOnTextChanged(this,
                    new TextChangedEventArgs(SubmitButton.Text, SubmitButton.Text));
            };
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            _entry = bindable;
            _entry.TextColor = Color.Gray;
            _entry.TextChanged += EntryOnTextChanged;
            SubmitButton.IsEnabled = false;
        }

        private void EntryOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (!_connectivity.IsConnected)
            {
                SubmitButton.IsEnabled = false;
                return;
            }

            if (textChangedEventArgs.NewTextValue.Length <= 1)
            {
                if (!Regex.IsMatch(textChangedEventArgs.NewTextValue, "[1-9]"))
                    _entry.Text = string.Empty;
            }
            else if (textChangedEventArgs.NewTextValue.Length >= 2
                && textChangedEventArgs.NewTextValue.Length <= _phoneNumberMaxLength)
            {
                var state = textChangedEventArgs.NewTextValue.Length > 4;
                _entry.TextColor = state ? Color.Black : Color.Gray;
                SubmitButton.IsEnabled = state;

                if (!Regex.IsMatch(textChangedEventArgs.NewTextValue, @"[1-9]\d+"))
                    _entry.Text = textChangedEventArgs.OldTextValue;
            }
            else
            {
                _entry.Text = textChangedEventArgs.OldTextValue;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            _entry.TextChanged -= EntryOnTextChanged;
        }
    }
}
