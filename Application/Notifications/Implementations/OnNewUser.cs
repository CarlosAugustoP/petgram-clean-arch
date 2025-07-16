using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.NotificationAggregate;

namespace Application.Notifications.Implementations
{
    public class OnNewUser : INotification
    {
        private List<Notification> _notifications = new List<Notification>();
        private readonly NotificationService _notificationService;
        private static readonly NotificationTrigger _type = NotificationTrigger.NEW_USER;

        public OnNewUser(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public record OnNewUserContext(string UserName, Guid UserId);

        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not OnNewUserContext context)
            {
                throw new ArgumentException("Invalid data for OnNewUser");
            }
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = $"Welcome {context.UserName}! Thank you for joining us.",
                Type = _type,
                ReceiverId = context.UserId,
                IsRead = false
            };
            _notifications.Add(notification);
        }

        public Task SendAsync(CancellationToken cancellationToken)
        {
            return _notificationService.CreateMany(cancellationToken, _notifications, _type);
        }
    }
}