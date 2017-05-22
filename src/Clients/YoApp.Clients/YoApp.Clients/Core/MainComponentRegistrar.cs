using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Autofac;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;
using Module = Autofac.Module;

namespace YoApp.Clients.Core
{
    /// <summary>
    /// Class that contains and builds this apps main componontents for AutoFac.
    /// </summary>
    public class MainComponentRegistrar : Module
    {
        /// <summary>
        /// Build the main component container for this app.
        /// </summary>
        /// <returns>Container that contains all main components.</returns>
        public static async Task<IContainer> BuildContainerAsync()
        {
            var builder = new ContainerBuilder();
            await Task.Run(() => builder.RegisterModule<MainComponentRegistrar>());
            return builder.Build();
        }

        protected override void Load(ContainerBuilder builder)
        {
            //Misc
            builder.RegisterType<AppSettings>().As<IStartable>().AsSelf().SingleInstance();
            builder.RegisterType<ChatBook>().AsSelf();
            builder.RegisterType<StateMachine.StateMachineController>().AsSelf().SingleInstance();
            builder.RegisterInstance(CrossContacts.Current).As<IContacts>().SingleInstance();
            builder.RegisterInstance(UserDialogs.Instance).As<IUserDialogs>().SingleInstance();

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
        }
    }
}
