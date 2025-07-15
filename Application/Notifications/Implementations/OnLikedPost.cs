using Domain.Models;
using Domain.Models.NotificationAggregate;
using Domain.Models.UserAggregate;

namespace Application.Notifications.Implementations
{
    public class OnLikedPost : INotification
    {
        private static readonly NotificationTrigger _type = NotificationTrigger.LIKED_POST;
        private readonly NotificationService _notificationService;
        private readonly List<Notification> _notifications = new();
        public OnLikedPost(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public record OnLikedPostContext(Post Post, User Liker, User Creator);
        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not OnLikedPostContext context)
            {
                throw new ArgumentException("Invalid data for OnLikedPost");
            }
            var lCount = context.Post.Likes.Count;


            if (lCount % 10 != 0 && lCount > 15)
            {
                return;
            }

            string? title = null;

            if (context.Post.Likes.Count >= 10)
            {
                title = $"Your post has gotten {context.Post.Likes.Count} likes!";
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = title ?? $"{context.Liker.Name} liked your post: {context.Post.Title}!",
                Type = _type,
                Receiver = context.Creator,
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