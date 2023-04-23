using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    public async Task SendUpdateNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveUpdateNotification", message);
    }

    public async Task SendDeleteNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveDeleteNotification", message);
    }
}
