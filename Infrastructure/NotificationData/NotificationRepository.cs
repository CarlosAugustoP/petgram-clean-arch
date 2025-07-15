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
    
        public async Task AddAsync(Notification notification)
        {
            await _db.Notifications.AddAsync(notification);
            await _db.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Notification> notifications)
        {
            await _db.Notifications.AddRangeAsync(notifications);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid notificationId)
        {
            var notification = await _db.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _db.Notifications.Remove(notification);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            //not paginating. Ill just get 20 at a time.
            return await _db.Notifications
                            .Where(n => n.ReceiverId == userId)
                            .OrderBy(n => n.IsRead) 
                            .ThenByDescending(n => n.SentAt)
                            .Take(20)
                            .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _db.Notifications
                            .CountAsync(n => n.Id == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notification = await _db.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                _db.Notifications.Update(notification);
                await _db.SaveChangesAsync();
            }
        }

        public Task MarkManyAsReadAsync(IEnumerable<Guid> notificationIds)
        {
            return _db.Notifications
                      .Where(n => notificationIds.Contains(n.Id) && !n.IsRead)
                      .ExecuteUpdateAsync(n => n.SetProperty(n => n.IsRead, true));
        }
    }
}