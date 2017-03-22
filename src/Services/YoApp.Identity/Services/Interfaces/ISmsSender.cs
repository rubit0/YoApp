using System.Threading.Tasks;

namespace YoApp.Identity.Services.Interfaces
{
    public interface ISmsSender
    {
        Task<bool> SendMessageAsync(string number, string message);
    }
}
