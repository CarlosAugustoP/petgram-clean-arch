using Domain.Models.NotificationAggregate;

namespace Domain.Repositorys
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification, CancellationToken cancellationToken);
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken);
        Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken);
        Task MarkManyAsReadAsync(IEnumerable<Guid> notificationIds, CancellationToken cancellationToken);
        Task DeleteAsync(Guid notificationId, CancellationToken cancellationToken);
        Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken);
    }
}