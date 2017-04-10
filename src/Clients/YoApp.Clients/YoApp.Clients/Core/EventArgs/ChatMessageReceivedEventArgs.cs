namespace YoApp.Clients.Core.EventArgs
{
    public class ChatMessageReceivedEventArgs : System.EventArgs
    {
        public string Sender { get; set; }
        public string Message { get; set; }
    }
}
