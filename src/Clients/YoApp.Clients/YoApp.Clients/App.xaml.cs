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
        public static IResolver Resolver { get; private set; }
        public static IResolver StorageResolver { get; private set; }
        public static ChatService ChatService { get; private set; }

        private StateMachine.StateMachine _stateMachine;
        private bool _canResume;

        public App()
        {
            InitializeComponent();

            Init();

            if (Settings.SetupFinished || ResourceKeys.IsDebug)
                MainPage = new NavigationPage(new Pages.MainPage());
            else
                MainPage = new NavigationPage(new Pages.Setup.WelcomePage());
        }

        private void Init()
        {
            StorageResolver = SetupStorageContainer().GetResolver();
            Settings = AppSettings.InitAppSettings();
            Resolver = SetupContainer().GetResolver();

            _stateMachine = new StateMachine.StateMachine();
            ChatService = new ChatService();
        }

        protected override async void OnStart()
        {
            await Task.Run(() =>
                {
                    MessagingCenter.Send(this, MessagingEvents.LifecycleChanged,
                        new LifecycleEventArgs(Lifecycle.Start));
                }).ConfigureAwait(false);
        }

        protected override async void OnSleep()
        {
            if (!_canResume)
                _canResume = true;

            await Task.Run(() =>
                {
                    MessagingCenter.Send(this, MessagingEvents.LifecycleChanged,
                        new LifecycleEventArgs(Lifecycle.Sleep));
                }).ConfigureAwait(false);
        }

        protected override async void OnResume()
        {
            //WORKAROUND: Prevent OnResume to be called at App start
            if (!_canResume)
                return;

            await Task.Run(() =>
                {
                    MessagingCenter.Send(this, MessagingEvents.LifecycleChanged,
                        new LifecycleEventArgs(Lifecycle.Resume));
                }).ConfigureAwait(false);

            //TODO: Check notifications and pending tasks
        }

        private IDependencyContainer SetupStorageContainer()
        {
            var container = new XLabs.Ioc.SimpleContainer();

            container.RegisterSingle<IKeyValueStore, AkavacheContext>();
            container.RegisterSingle<IRealmStore, RealmContext>();

            return container;
        }

        private IDependencyContainer SetupContainer()
        {
            var container = new XLabs.Ioc.SimpleContainer();

            container.RegisterSingle<IAppUserManager, AppUserManager>();
            container.RegisterSingle<IContactsManager, ContactsManager>();
            container.RegisterSingle<IFriendsManager, FriendsManager>();
            container.Register<IVerificationManager>(typeof(VerificationManager));

            return container;
        }
    }
}
