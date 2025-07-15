using Microsoft.AspNetCore.SignalR;
using Application.Notifications.DTOs;

namespace Application.Notifications.WebSockets
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(Guid userId, NotificationDTO notification)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
        }
    }

    public class NotificationDTO
    {
    }
}
