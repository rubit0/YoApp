using FluentScheduler;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
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
    public class StateMachine : IDisposable
    {
        private readonly LifeCycleState _lifeCycleState;
        private readonly UserCreatedState _setupFinishedState;
        private readonly SchedulerState _schedulerState;
        private readonly ConnectivityState _connectivityState;

        public StateMachine()
        {
            _lifeCycleState = new LifeCycleState();
            _setupFinishedState = new UserCreatedState();
            _schedulerState = new SchedulerState();
            _connectivityState = new ConnectivityState();

            SetupSubscriptions();
        }

        private void SetupSubscriptions()
        {
            CrossConnectivity.Current.ConnectivityChanged +=
                async (s, e) => await OnConnectivityChanged(e);

            MessagingCenter.Subscribe<VerificationViewModel>(this, MessagingEvents.UserCreated,
                async (s) => await OnUserCreated());

            MessagingCenter.Subscribe<App, LifecycleEventArgs>(this, MessagingEvents.LifecycleChanged,
                async (s, e) => await OnLifeCycleChanged(e));
        }

        private async Task OnLifeCycleChanged(LifecycleEventArgs eventArgs)
        {
            _schedulerState.HandleState(eventArgs.State);
            await _lifeCycleState.HandleState(eventArgs.State);
        }

        private async Task OnUserCreated()
        {
            await _setupFinishedState.Execute();
        }

        private async Task OnConnectivityChanged(ConnectivityChangedEventArgs eventArgs)
        {
            await _connectivityState.HandleConnectivityState(eventArgs.IsConnected);
        }

        public void Dispose()
        {
            CrossConnectivity.Current.ConnectivityChanged -=
                async (s, e) => await OnConnectivityChanged(e);

            MessagingCenter.Unsubscribe<VerificationViewModel>(this, MessagingEvents.UserCreated);
            MessagingCenter.Unsubscribe<App, LifecycleEventArgs>(this, MessagingEvents.LifecycleChanged);
        }
    }
}
