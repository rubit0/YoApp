using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoApp.Clients.Helpers.EventArgs
{
    public class ChatMessageReceivedEventArgs : System.EventArgs
    {
        public string Sender { get; set; }
        public string Message { get; set; }
    }
}
