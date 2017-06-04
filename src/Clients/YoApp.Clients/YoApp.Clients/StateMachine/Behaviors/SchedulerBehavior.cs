using System.Threading.Tasks;
using FluentScheduler;
using YoApp.Clients.Services;

namespace YoApp.Clients.StateMachine.Behaviors
{
    public class SchedulerBehavior : AppBehavior
    {
        public override async Task OnStart()
        {
            await Task.Run(() => JobManager.Initialize(BuildMainRegistry()));
        }

        public override async Task OnSleep()
        {
            await Task.Run(() => JobManager.Stop());
        }

        public override async Task OnResume()
        {
            await Task.Run(() => JobManager.Start());
        }

        private Registry BuildMainRegistry()
        {
            var registry = new Registry();

            registry.Schedule(async ()=> 
            {
                if (AuthenticationService.CanRequestToken())
                    await AuthenticationService.RequestToken(true);
            }).ToRunEvery(3300).Seconds();

            return registry;
        }
    }
}
