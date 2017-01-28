using System.Threading.Tasks;

namespace YoApp.Backend.Services.Interfaces
{
    public interface ISmsSender
    {
        Task<bool> SendMessageAsync(string number, string message);
    }
}
