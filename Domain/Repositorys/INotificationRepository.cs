using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Notification;

namespace Domain.Repositorys
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
        Task DeleteAsync(Guid notificationId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task AddRangeAsync(IEnumerable<Notification> notifications);
    }
}