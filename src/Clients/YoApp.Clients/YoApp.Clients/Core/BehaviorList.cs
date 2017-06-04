using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoApp.Clients.StateMachine;

namespace YoApp.Clients.Core
{
    public class BehaviorList : List<AppBehavior>
    {
        public BehaviorList()
        {
            
        }

        public BehaviorList(IEnumerable<AppBehavior> behaviors)
        {
            this.AddRange(behaviors);
        }

        public async Task InvokeStart()
        {
            var tasks = this.Select(ab => ab.OnStart()).ToArray();
            await Task.WhenAll(tasks);
        }

        public async Task InvokeSleep()
        {
            var tasks = this.Select(ab => ab.OnSleep()).ToArray();
            await Task.WhenAll(tasks);
        }

        public async Task InvokeResume()
        {
            var tasks = this.Select(ab => ab.OnResume()).ToArray();
            await Task.WhenAll(tasks);
        }

        public async Task InvokeSetupComplete()
        {
            var tasks = this.Select(ab => ab.OnSetupComplete()).ToArray();
            await Task.WhenAll(tasks);
        }

        public async Task InvokeConnectivity(bool state)
        {
            var tasks = this.Select(ab => ab.OnConnectivityChanged(state)).ToArray();
            await Task.WhenAll(tasks);
        }

        public async Task SendMessage(object message)
        {
            var tasks = this.Select(ab => ab.OnMessageReceived(message)).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
