using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanacealogicsSales.Entities.Models
{
    public class MessageHub : Hub
    {
        static List<string> groups = new List<string>();
        public  static List<string> connectionlist = new List<string>();
        public static string conId = "";
        public override async Task OnConnectedAsync()
        {
             await Groups.AddToGroupAsync(Context.ConnectionId, "allusers");
             conId = Context.ConnectionId;
             connectionlist.Add(conId);
           

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "allusers");
           await base.OnDisconnectedAsync(exception);
        }

        

    }
}
