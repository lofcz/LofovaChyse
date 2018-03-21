using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Models;
using Microsoft.AspNet.SignalR;

namespace LofovaChyse
{
    public class ChatHub : Hub
    {
        static List<KnihovnaUser> users = new List<KnihovnaUser>();

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (users.Count(x => x._ChatID == id) == 0)
            {
                string logintime = DateTime.Now.ToString();
                users.Add(new KnihovnaUser() {_ChatID = id, Name = userName});

                Clients.Caller.onConnected(id, userName, users);

                Clients.AllExcept(id).onNewUserConnected(id, userName, logintime);
            }
        }

    }
}