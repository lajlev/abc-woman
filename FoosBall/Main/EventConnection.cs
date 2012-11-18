namespace FoosBall.Main
{
    using System.Threading.Tasks;

    using SignalR;

    public class EventConnection : PersistentConnection 
    {
        protected override Task OnConnectedAsync(IRequest request, string connectionId)
        {
            Connection.Broadcast("Connection " + connectionId + " connected");
            return Groups.Add(connectionId, "foo");
        }

        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            // Broadcast data to all clients
            return Connection.Broadcast("Connection " + connectionId + " sent " + data);
        }

        protected override Task OnDisconnectAsync(string connectionId)
        {
            return Groups.Remove(connectionId, "foo");
        }
    }
}