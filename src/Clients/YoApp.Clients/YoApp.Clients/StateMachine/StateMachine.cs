using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Helpers.EventArgs;
using YoApp.Clients.StateMachine.States;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.StateMachine
{
    /// <summary>
    /// Manages all relevant states of the App.
    /// </summary>
    public class StateMachine
    {
        private readonly LifeCycleState _lifeCycleState;
        private readonly UserCreatedState _setupFinishedState;

        public StateMachine()
        {
            _lifeCycleState = new LifeCycleState();
            _setupFinishedState = new UserCreatedState();

            MessagingCenter.Subscribe<VerificationViewModel>(this, MessagingEvents.UserCreated,
                async (s) => await OnUserCreated());

            MessagingCenter.Subscribe<App, LifecycleEventArgs>(this, MessagingEvents.LifecycleChanged,
                async (s, e) => await OnLifeCycleChanged(e));
        }

        private async Task OnLifeCycleChanged(LifecycleEventArgs eventArgs)
        {
            await _lifeCycleState.HandleState(eventArgs.State);
        }

        private async Task OnUserCreated()
        {
            await _setupFinishedState.Execute();
        }
    }
}
