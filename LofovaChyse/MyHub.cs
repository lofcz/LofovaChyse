using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;
using Microsoft.AspNet.SignalR;

namespace LofovaChyse
{
    public class MyHub : Hub
    {
        public MyHub()
        {
            // Create a Long running task to do an infinite loop which will keep sending the server time
            // to the clients every 3 seconds.
            var taskTimer = Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        string timeNow = DateTime.Now.ToString();
                        //Sending the server time to all the connected clients on the client method SendServerTime()
                        Clients.All.SendServerTime(timeNow);
                        //Delaying by 3 seconds.
                        await Task.Delay(3000);
                    }
                }, TaskCreationOptions.LongRunning
            );
        }

        public void HelloServer(string name, string text)
        {
            ChatZpravy z = new ChatZpravy();
            z.Id = Books.Counter();
            z.Date = DateTime.Now;
            z.Text = text;
            z.UserFrom = new KnihovnaUserDao().GetByLogin(name).Id;

            ChatZpravyDao d = new ChatZpravyDao();
            d.Create(z);

            Clients.All.hello(name + ": " + text);
        }
    }
}