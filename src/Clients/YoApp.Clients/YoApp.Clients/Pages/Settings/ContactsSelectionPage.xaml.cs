using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class ContactsSelectionPage : ContentPage, IPageService
    {
        public ContactsSelectionPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<ContactsSelectionViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }
    }
}
