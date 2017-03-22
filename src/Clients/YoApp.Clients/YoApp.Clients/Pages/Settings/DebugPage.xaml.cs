using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.ViewModels.Settings;

namespace YoApp.Clients.Pages.Settings
{
    public partial class DebugPage : ContentPage, IPageService
    {
        private readonly DebugPageViewModel _viewModel;

        public DebugPage()
        {
            InitializeComponent();
            _viewModel = new DebugPageViewModel(this);
            BindingContext = _viewModel;
        }
    }
}
