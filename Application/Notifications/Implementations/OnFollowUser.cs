using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.NotificationAggregate;
using Domain.Models.UserAggregate;

namespace Application.Notifications.Implementations
{
    public class OnFollowUser : INotification
    {
        private readonly NotificationService _notificationService;
        private readonly List<Notification> _notifications = new List<Notification>();
        private static readonly NotificationTrigger _type = NotificationTrigger.NEW_FOLLOWER;

        public OnFollowUser(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public record OnFollowUserContext(User Follower, Guid FollowedUserId);

        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not OnFollowUserContext context)
            {
                throw new ArgumentException("Invalid data for OnFollowUser");
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = $"{context.Follower.Name} has started following you!",
                Type = _type,
                ReceiverId = context.FollowedUserId,
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