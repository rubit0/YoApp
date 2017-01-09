using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Backend.Services.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessageAsync(string number, string message);
    }
}
