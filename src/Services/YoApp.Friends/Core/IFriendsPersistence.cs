using YoApp.Data.Repositories;

namespace YoApp.Friends.Core
{
    public interface IFriendsPersistence
    {
        IFriendsRepository Friends { get; }
    }
}