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
        
        public void PushMessage(IRealmStore store, ChatMessage message)
        {
            store.Instance.Write(() =>
            {
                Messages.Add(message);
            });
        }
    }
}
