using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using YoApp.Clients.Models;

namespace YoApp.Clients.Manager
{
    public interface IFriendsManager : INotifyPropertyChanged
    {
        ObservableCollection<Friend> Friends { get; }

        Task LoadFriends();
        Task ManageFriends(List<LocalContact> contacts);
        Task DiscoverFriendsAsync(List<LocalContact> contacts);
        void MatchFriendsToContacts(IEnumerable<Friend> friends, List<LocalContact> contacts);
        Task RemoveStaleFriendsAsync(List<LocalContact> contacts);
    }
}