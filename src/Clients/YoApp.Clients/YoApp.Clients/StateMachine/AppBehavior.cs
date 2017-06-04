using System.Threading.Tasks;

namespace YoApp.Clients.StateMachine
{
    public abstract class AppBehavior
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnStart()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnSleep()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnResume()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnSetupComplete()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnConnectivityChanged(bool isConnected)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnMessageReceived(object message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return;
        }
    }
}
