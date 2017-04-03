using YoApp.Data.Repositories;

namespace YoApp.Friends.Helper
{
    public interface IFriendsPersistence
    {
        IFriendsRepository Friends { get; }
    }
}