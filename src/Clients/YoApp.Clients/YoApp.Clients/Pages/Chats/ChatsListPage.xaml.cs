using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Chats;

namespace YoApp.Clients.Pages.Chats
{
    public partial class ChatsListPage : ContentPage, IPageService
    {
        public ChatsListPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            BindingContext = App.Container.Resolve<ChatsListViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }

        protected override void OnAppearing()
        {
            ListView.SelectedItem = null;
        }
    }
}
