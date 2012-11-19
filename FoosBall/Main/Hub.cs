namespace FoosBall.Main
{
    using System;
    using System.Web.Routing;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    public class Chat : Hub
    {
        public void Send(string message)
        {
            // Call the addMessage method on all clients            
            Clients.All.addMessage(message);
        }
    }

}