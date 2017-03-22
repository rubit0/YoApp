using Akavache;
using Realms;
using System.Reactive.Linq;
using System.Threading.Tasks;
using YoApp.Clients.Models;

namespace YoApp.Clients.Helpers
{
    public class PersistenceHelpers
    {
        public static async Task DropTables()
        {
            await BlobCache.UserAccount
                .InvalidateAllObjects<Friend>();

            await Realm.GetInstance().WriteAsync((i) =>
            {
                i.RemoveAll<ChatMessage>();
                i.RemoveAll<ChatBook>();
            });
        }
    }
}
