using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class DebugPage : ContentPage, IPageService
    {
        public DebugPage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<DebugViewModel>(
                new TypedParameter(typeof(IPageService), this));
        }
    }
}
