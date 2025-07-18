using Domain.Models.NotificationAggregate;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.NotificationData
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MainDBContext _db;
        public NotificationRepository(MainDBContext db)
        {
            _db = db;
        }
    
        public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
        {
            await _db.Notifications.AddAsync(notification, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken)
        {
            await _db.Notifications.AddRangeAsync(notifications, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            var notification = await _db.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _db.Notifications.Remove(notification);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            return await _db.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            //not paginating. Ill just get 20 at a time.
            return await _db.Notifications
                            .Where(n => n.ReceiverId == userId)
                            .OrderByDescending(n => n.SentAt) 
                            .Take(20)
                            .ToListAsync(cancellationToken);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _db.Notifications
                            .CountAsync(n => n.Id == userId && !n.IsRead, cancellationToken);
        }

        public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            var notification = await _db.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                _db.Notifications.Update(notification);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public Task MarkManyAsReadAsync(IEnumerable<Guid> notificationIds, CancellationToken cancellationToken)
        {
            return _db.Notifications
                      .Where(n => notificationIds.Contains(n.Id) && !n.IsRead)
                      .ExecuteUpdateAsync(n => n.SetProperty(n => n.IsRead, true), cancellationToken);
        }
    }
}