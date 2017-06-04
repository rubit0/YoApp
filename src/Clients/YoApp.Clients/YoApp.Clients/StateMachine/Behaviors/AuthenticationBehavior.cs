using System.Threading.Tasks;
using YoApp.Clients.Services;

namespace YoApp.Clients.StateMachine.Behaviors
{
    public class AuthenticationBehavior : AppBehavior
    {
        public override async Task OnStart()
        {
            if (AuthenticationService.CanRequestToken())
                await AuthenticationService.RequestToken(true);
        }

        public override async Task OnResume()
        {
            if (!App.Settings.SetupFinished)
                return;

            if (AuthenticationService.CanRequestToken())
                await AuthenticationService.RequestToken(true);
        }

        public override async Task OnConnectivityChanged(bool isConnected)
        {
            if (isConnected)
            {
                if (AuthenticationService.AuthAccount != null)
                    await AuthenticationService.RequestToken(true);
            }
        }
    }
}
