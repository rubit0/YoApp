using System.Collections.ObjectModel;
using System.Threading.Tasks;
using YoApp.Clients.Models;
using YoApp.Clients.Pages.Chats;

namespace YoApp.Clients.Manager
{
    public interface IChatManager
    {
        ObservableCollection<ChatPage> Pages { get; set; }

        ChatPage GetPageByFriend(Friend friend);
        Task OpenChat(Friend friend);
    }
}