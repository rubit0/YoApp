using System;

using Xamarin.Forms;

namespace YoApp.Clients.Pages.Setup
{
    public partial class CompletePage : ContentPage
    {
        public CompletePage()
        {
            InitializeComponent();
        }

        private void OnDoneClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
