using System.Threading.Tasks;
using Xamarin.Forms;

namespace YoApp.Clients.Helpers
{
    public interface IPageService
    {
        INavigation Navigation { get; }
        Task DisplayAlert(string title, string messsage, string ok);
        Task<bool> DisplayAlert(string title, string message, string cancel, string ok);
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
    }
}
