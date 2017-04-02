using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using YoApp.Clients.Helpers;
using YoApp.Clients.Helpers.EventArgs;
using YoApp.Clients.Manager;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients
{
    public partial class App : Application
    {
        public static AppSettings Settings { get; private set; }
        public static IResolver Managers { get; private set; }
        public static IResolver Persistence { get; private set; }
        public static IResolver Services { get; private set; }

        private StateMachine.StateMachine _stateMachine;

        public App()
        {
            InitializeComponent();
            InitApp();

            MainPage = GetMainPage();
        }

        protected override void OnStart()
        {
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

        #region Setup and Helpers
        private void InitApp()
        {
            Persistence = SetupStorageContainer().GetResolver();

            Settings = AppSettings.InitAppSettings();

            Services = SetupServicesContainer().GetResolver();
            Managers = SetupManagerContainer().GetResolver();

            _stateMachine = new StateMachine.StateMachine();
        }

        private Page GetMainPage()
        {
            if (Settings.SetupFinished || ResourceKeys.IsDebug)
                return new NavigationPage(new Pages.MainPage());
            else
                return new NavigationPage(new Pages.Setup.WelcomePage());
        }

        private IDependencyContainer SetupManagerContainer()
        {
            var container = new SimpleContainer();

            container.RegisterSingle<IAppUserManager, AppUserManager>();
            container.RegisterSingle<IContactsManager, ContactsManager>();
            container.RegisterSingle<IFriendsManager, FriendsManager>();
            container.RegisterSingle<IChatManager, ChatManager>();
            container.Register<IVerificationManager>(typeof(VerificationManager));

            return container;
        }

        private IDependencyContainer SetupStorageContainer()
        {
            var container = new SimpleContainer();

            container.RegisterSingle<IKeyValueStore, AkavacheContext>();
            container.RegisterSingle<IRealmStore, RealmContext>();

            return container;
        }

        private IDependencyContainer SetupServicesContainer()
        {
            var container = new SimpleContainer();

            container.RegisterSingle<IAccountService, AccountService>();
            container.RegisterSingle<IFriendsService, FriendsService>();
            container.RegisterSingle<ChatService, ChatService>();

            return container;
        }

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
