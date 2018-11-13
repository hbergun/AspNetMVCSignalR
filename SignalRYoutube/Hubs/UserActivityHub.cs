using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRYoutube.Hubs
{
    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {
        public static List<string> Users = new List<string>();

        public void Send(int count)
        {
           // var context = GlobalHost.ConnectionManager.GetHubContext<UserActivityHub>();
            //context.Clients.All.updateUsersOnlineCount(count);
            Clients.All.updateUsersOnlineCount(count);
        }

        public override Task OnConnected()
        {
            string clientId = GetClientId();
            FileOperation.ServerLogs("User Connected:[ClientID]    " + clientId + " Time: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"+"<br/>"));

            if (Users.IndexOf(clientId) == -1) //Users Listesi ClientId'yi İçermiyorsa Ekle
                Users.Add(clientId);

            Send(Users.Count);
            return base.OnConnected();
        }


       public override Task OnDisconnected(bool StopCalled)
       {
           string clientId = GetClientId();
           if (Users.IndexOf(clientId) > -1) //Users Listesi ClientID'yi içeriyorsa sil
            {
                Users.Remove(clientId);
                FileOperation.ServerLogs("User DisConnected:[ClientID] " + clientId+" Time: "+ DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"+"<br/>"/*DateTime.Now.Date.ToString("dd/MM/yyyy")*/));

            }

            Send(Users.Count);
           return base.OnDisconnected(StopCalled);
       }


        public override Task OnReconnected() //Is Not Run
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) == -1) //User Listesi Client Id'yi İçermiyorsa Ekle
            {
                Users.Add(clientId);
                FileOperation.ServerLogs("User ReConnected:[ClientID]" + clientId);
            }

            Send(Users.Count);

            return base.OnReconnected();
        }

        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
                clientId = this.Context.QueryString["clientId"];
            
            if (string.IsNullOrEmpty(clientId.Trim()))
                clientId = Context.ConnectionId;
            
            return clientId;
        }
    }
}