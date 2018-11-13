using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRYoutube.Hubs
{
    public class MyHub : Hub
    {
        public override async Task OnConnected() //async
        {
            await Clients.Caller.getConnectionID(Context.ConnectionId);
          
        }

        public void Anons(string userName,string messages)
        {
            string value = "<b>" + messages + " : </b>" + userName;
            Clients.All.anons(value+"<br/>"); //Burda anons Anons fark etmez ama cshtml de galiba fark ediyor
            FileOperation.ChatLogs(value+" Time:"+DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") +"<br/>");

        }
       
        public int ReturnDate()
        {
            return DateTime.Now.Year;
        }

       

    }
}