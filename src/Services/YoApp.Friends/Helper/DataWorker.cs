using YoApp.Data;
using YoApp.Data.Repositories;

namespace YoApp.Friends.Helper
{
    public class DataWorker : UnitOfWorkBase
    {
        public IFriendsRepository Friends { get; }

        public DataWorker(ApplicationDbContext context, IFriendsRepository friendsRepository) : base(context)
        {
            Friends = friendsRepository;
        }
    }
}
