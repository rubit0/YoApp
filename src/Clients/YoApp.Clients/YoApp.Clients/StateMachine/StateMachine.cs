using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Core.EventArgs;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.StateMachine
{
    /// <summary>
    /// Propagates app states to the underlying behaviors.
    /// </summary>
    public class StateMachine : IDisposable, IStartable
    {
        private readonly BehaviorList _behaviors;

        public StateMachine(IEnumerable<AppBehavior> behaviors)
        {
            _behaviors = new BehaviorList(behaviors);
        }

        private async Task OnLifeCycleChanged(LifecycleEventArgs eventArgs)
        {
            switch (eventArgs.State)
            {
                case Lifecycle.Start:
                    await _behaviors.InvokeStart();
                    break;
                case Lifecycle.Sleep:
                    await _behaviors.InvokeSleep();
                    break;
                case Lifecycle.Resume:
                    await _behaviors.InvokeResume();
                    break;
                case Lifecycle.SetupCompleted:
                    await _behaviors.InvokeSetupComplete();
                    break;
                default:
                    break;
            }
        }

        private async Task OnConnectivityChanged(ConnectivityChangedEventArgs eventArgs)
        {
            await _behaviors.InvokeConnectivity(eventArgs.IsConnected);
        }

        public void Start()
        {
            CrossConnectivity.Current.ConnectivityChanged +=
                async (s, e) => await OnConnectivityChanged(e);

            MessagingCenter.Subscribe<App, LifecycleEventArgs>(this, MessagingEvents.LifecycleChanged,
                async (s, e) => await OnLifeCycleChanged(e));

            if (!App.Settings.SetupFinished)
            {
                MessagingCenter.Subscribe<VerificationViewModel, LifecycleEventArgs>(this, MessagingEvents.LifecycleChanged,
                    async (s, e) => await OnLifeCycleChanged(e));
            }
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<VerificationViewModel>(this, MessagingEvents.UserCreated);
            MessagingCenter.Unsubscribe<App, LifecycleEventArgs>(this, MessagingEvents.LifecycleChanged);
        }
    }
}
