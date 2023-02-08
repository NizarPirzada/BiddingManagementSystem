using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PanacealogicsSales.Entities.Models
{
    public class NotifyMessage
    {
        public string Message { get; set; }
    }
    public  class MessageHubs :Hub
    {
        public async Task SendMessage(NotifyMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
