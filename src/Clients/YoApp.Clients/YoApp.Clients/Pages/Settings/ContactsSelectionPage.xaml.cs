
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class ContactsSelectionPage : ContentPage, IPageService
    {
        public ContactsSelectionPage()
        {
            InitializeComponent();
            BindingContext = new ContactsSelectionViewModel(this);
        }
    }
}
