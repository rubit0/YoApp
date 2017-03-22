using Xamarin.Forms;

namespace YoApp.Clients.Pages.Modals
{
    public partial class LoadingModalPage : ContentPage
    {
        public LoadingModalPage(string message)
        {
            InitializeComponent();
            BindingContext = this;
            MessageLabel.Text = message;
        }
    }
}
