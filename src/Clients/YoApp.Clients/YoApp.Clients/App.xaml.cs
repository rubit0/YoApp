using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Core.EventArgs;
using YoApp.Clients.Pages;

namespace YoApp.Clients
{
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        public static AppSettings Settings { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new SplashPage();
        }

        protected override async void OnStart()
        {
            Container = await MainComponentRegistrar.BuildContainerAsync();
            Settings = Container.Resolve<AppSettings>();
            Container.Resolve<StateMachine.StateMachineController>().Start();

            SendLifeCycleEvent(Lifecycle.Start);
        }

        protected override void OnSleep()
        {
            SendLifeCycleEvent(Lifecycle.Sleep);
        }

        protected override void OnResume()
        {
            SendLifeCycleEvent(Lifecycle.Resume);
        }

        #region Helpers
        private async void SendLifeCycleEvent(Lifecycle state)
        {
            await Task.Run(() =>
            {
                MessagingCenter.Send(this, MessagingEvents.LifecycleChanged,
                    new LifecycleEventArgs(state));
            }).ConfigureAwait(false);
        }
        #endregion
    }
}
