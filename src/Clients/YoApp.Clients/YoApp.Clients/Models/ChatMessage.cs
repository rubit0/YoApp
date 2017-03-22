using Realms;
using System;

namespace YoApp.Clients.Models
{
    public class ChatMessage : RealmObject
    {
        public enum Delivery
        {
            Pending,
            Send,
            Received
        }

        public bool IsIncomming { get; set; }
        public int DeliveryState { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
