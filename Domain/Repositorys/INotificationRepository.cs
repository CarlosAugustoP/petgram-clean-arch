using Domain.Models.NotificationAggregate;

namespace Domain.Repositorys
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
        Task MarkManyAsReadAsync(IEnumerable<Guid> notificationIds);
        Task DeleteAsync(Guid notificationId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task AddRangeAsync(IEnumerable<Notification> notifications);
    }
}