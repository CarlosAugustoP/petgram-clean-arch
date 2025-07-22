using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Domain.Models.NotificationAggregate;
using Domain.Models.UserAggregate;
using Notification = Domain.Models.NotificationAggregate.Notification;

namespace Application.Notifications.Implementations
{
    public class OnInactiveUserReminded : INotification
    {
        private List<Notification> _notifications = new List<Notification>();
        private readonly NotificationService _notificationService;
        private static readonly NotificationTrigger _type = NotificationTrigger.INACTIVE_USER_REMINDER;

        public OnInactiveUserReminded(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not User context)
            {
                throw new ArgumentException("Invalid data for OnInactiveUserReminded");
            }
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = $"Hi {context.Name}, we noticed you haven't been active recently. We'd love to see you back!",
                Type = _type,
                ReceiverId = context.Id,
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