using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Helpers.EventArgs;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;

namespace YoApp.Clients
{
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        public static AppSettings Settings { get; private set; }

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
            Container = BuildContainer();
            Settings = Container.Resolve<AppSettings>();
            _stateMachine = new StateMachine.StateMachine();
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            //Settings
            builder.RegisterType<AppSettings>().As<IStartable>().AsSelf().SingleInstance();
            builder.RegisterType<ChatBook>().AsSelf();

            //Persistence
            builder.RegisterType<AkavacheContext>().As<IKeyValueStore>().SingleInstance();
            builder.RegisterType<RealmContext>().As<IRealmStore>().SingleInstance();

            //Services
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<FriendsService>().As<IFriendsService>().SingleInstance();
            builder.RegisterType<ChatService>().As<IChatService>().SingleInstance();

            //Managers
            builder.RegisterType<AppUserManager>().As<IAppUserManager>().SingleInstance();
            builder.RegisterType<ContactsManager>().As<IContactsManager>().SingleInstance();
            builder.RegisterType<FriendsManager>().As<IFriendsManager>().SingleInstance();
            builder.RegisterType<ChatManager>().As<IChatManager>().SingleInstance();
            builder.RegisterType<VerificationManager>().As<IVerificationManager>();

            //Scan Assemblies
            var current = typeof(App).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(current).Where(t => t.Name.EndsWith("State"));
            builder.RegisterAssemblyTypes(current).Where(t => t.Name.EndsWith("ViewModel"));

            return builder.Build();
        }

        private Page GetMainPage()
        {
            return (Settings.SetupFinished || ResourceKeys.IsDebug) 
                ? new NavigationPage(new Pages.MainPage()) 
                : new NavigationPage(new Pages.Setup.WelcomePage());
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
