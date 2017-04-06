using FluentScheduler;
using YoApp.Clients.Helpers.EventArgs;
using YoApp.Clients.Services;

namespace YoApp.Clients.StateMachine.States
{
    public class SchedulerState
    {
        public void HandleState(Lifecycle state)
        {
            switch (state)
            {
                case Lifecycle.Start:
                    SetupScheduler();
                    break;
                case Lifecycle.Sleep:
                    JobManager.Stop();
                    break;
                case Lifecycle.Resume:
                    JobManager.Start();
                    break;
                default:
                    break;
            }
        }

        private void SetupScheduler()
        {
            JobManager.Initialize(BuildMainRegistry());
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
