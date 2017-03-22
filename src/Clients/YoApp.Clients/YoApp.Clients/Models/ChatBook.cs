using Realms;
using System.Collections.Generic;
using YoApp.Clients.Persistence;

namespace YoApp.Clients.Models
{
    public class ChatBook : RealmObject
    {
        [PrimaryKey]
        public string FriendKey { get; set; }
        public IList<ChatMessage> Messages { get; }

        private readonly IRealmStore _store;

        public ChatBook()
        {
            _store = App.StorageResolver.Resolve<IRealmStore>();
        }
        
        public void PushMessage(ChatMessage message)
        {
            _store.Instance.Write(() =>
            {
                Messages.Add(message);
            });
        }
    }
}
