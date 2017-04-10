using System;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace YoApp.Clients.Forms.Behaviors
{
    public class ListViewAutoScroll : Behavior<ListView>
    {
        public bool ScrollUp { get; set; }
        private ListView _listView;
        private INotifyCollectionChanged _collection;

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            _listView = bindable;

            _listView.BindingContextChanged += OnListViewBindingContextChanged;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);

            _listView.BindingContextChanged -= OnListViewBindingContextChanged;
            if (_collection != null)
                _collection.CollectionChanged -= OnCollectionChanged;
        }

        private void OnListViewBindingContextChanged(object sender, EventArgs eventArgs)
        {
            _collection = _listView?.ItemsSource as INotifyCollectionChanged;
            if (_collection == null)
                throw new InvalidCastException($"The ItemSource must implement {nameof(INotifyCollectionChanged)}");

            _collection.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (ScrollUp)
            {
                var target = _listView?.ItemsSource.Cast<object>().First();
                _listView?.ScrollTo(target, ScrollToPosition.MakeVisible, true);
            }
            else
            {
                var target = _listView?.ItemsSource.Cast<object>().Last();
                _listView?.ScrollTo(target, ScrollToPosition.MakeVisible, true);
            }
        }
    }
}
