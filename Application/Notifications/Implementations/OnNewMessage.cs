using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.NotificationAggregate;
using Infrastructure.Migrations;
using Org.BouncyCastle.Tsp;

namespace Application.Notifications.Implementations
{
    public class OnNewMessage : INotification
    {
        private readonly NotificationService _notificationService;
        private readonly List<Notification> _notifications = new List<Notification>();
        private static readonly NotificationTrigger _type = NotificationTrigger.NEW_MESSAGE;

        public OnNewMessage(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public record OnNewMessageContext(Message Message);

        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not OnNewMessageContext context)
            {
                throw new ArgumentException("Invalid data for OnNewMessage");
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = $"{context.Message.Sender.Name} sent you a message: {FormatMessage(context.Message.Content)}",
                Type = _type,
                ReceiverId = context.Message.ReceiverId,
                IsRead = false
            };
            _notifications.Add(notification);
        }

        public Task SendAsync(CancellationToken cancellationToken)
        {
            return _notificationService.CreateMany(cancellationToken, _notifications, _type);
        }
        private static string FormatMessage(string content)
        {
            if (content.Length > 20)
            {
                return content.Substring(0, 20) + "...";
            }
            return content;
        }
        
    }
}