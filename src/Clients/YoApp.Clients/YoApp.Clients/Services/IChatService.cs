using System;
using System.Threading.Tasks;
using YoApp.Clients.Core.EventArgs;
using YoApp.Clients.Models;

namespace YoApp.Clients.Services
{
    public interface IChatService
    {
        event EventHandler<ChatMessageReceivedEventArgs> OnChatMessageReceived;

        Task<bool> Connect();
        Task<bool> SendMessage(Friend friend, string message);
    }
}