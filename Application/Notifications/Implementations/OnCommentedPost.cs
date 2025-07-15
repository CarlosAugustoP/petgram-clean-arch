using Domain.Models;
using Domain.Models.NotificationAggregate;

namespace Application.Notifications.Implementations
{
    public class OnCommentedPost : INotification
    {
        private readonly NotificationService _notificationService;
        private readonly List<Notification> _notifications = new();
        private static readonly NotificationTrigger _type = NotificationTrigger.COMMENTED_POST;

        public OnCommentedPost(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public record OnCommentedPostContext(Post Post, Comment Comment);

        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not OnCommentedPostContext context)
            {
                throw new ArgumentException("Invalid data for OnCommentedPost");
            }

            string? title = null;

            if (context.Comment.Post == null)
            {
                title = $"{context.Comment.Author!.Name} replied to your comment on {context.Post.Title}";
            }
            else
            {
                title = $"{context.Comment.Author!.Name} commented on your post: {context.Post.Title}";
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = title,
                Type = _type,
                Receiver = context.Post.Author,
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