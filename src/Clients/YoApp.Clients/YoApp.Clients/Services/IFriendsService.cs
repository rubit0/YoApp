using System.Collections.Generic;
using System.Threading.Tasks;
using YoApp.Clients.Models;

namespace YoApp.Clients.Services
{
    public interface IFriendsService
    {
        Task<bool> CheckMembership(string phoneNumber);
        Task<IEnumerable<string>> CheckMembershipRange(IEnumerable<string> phoneNumbers);
        Task<Friend> FetchFriend(string phoneNumber);
        Task<IEnumerable<Friend>> FetchFriends(IEnumerable<string> phoneNumbers);
        Task<string> GetName(string phoneNumber);
        Task<string> GetStatus(string phoneNumber);
    }
}