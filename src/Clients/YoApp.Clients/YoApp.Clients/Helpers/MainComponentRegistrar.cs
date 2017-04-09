using Autofac;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using YoApp.Clients.Manager;
using YoApp.Clients.Models;
using YoApp.Clients.Persistence;
using YoApp.Clients.Services;
using Module = Autofac.Module;

namespace YoApp.Clients.Helpers
{
    public class MainComponentRegistrar : Module
    {
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
