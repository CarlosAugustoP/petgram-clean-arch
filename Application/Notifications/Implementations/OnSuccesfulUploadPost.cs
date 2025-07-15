using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Notification;

namespace Application.Notifications.Implementations
{
    public class OnSuccessfulUploadPost : INotification
    {
        private readonly NotificationService _notificationService;
        private readonly NotificationTrigger _type = NotificationTrigger.POST_FINISHED_UPLOAD;
        private readonly List<Notification> _notifications = [];

        public OnSuccessfulUploadPost(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        
        public record OnSuccessfulUploadPostContext(Post Post, List<Pet> Pets, User Creator);

        public void Prepare(object data, CancellationToken cancellationToken)
        {
            if (data is not OnSuccessfulUploadPostContext context)
            {
                throw new ArgumentException("Invalid data for OnSuccessfulUploadPost");
            }

            foreach (var pet in context.Pets)
            {
                if (pet.Owner == null) throw new ArgumentException("Pet owner cannot be null");

                if (pet.Owner.Id == context.Creator.Id)
                {
                    continue;
                }

                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    SentAt = DateTime.UtcNow,
                    Message = $"A user created a post mentioning your pet {pet.Name} aren't they adorable?.",
                    Type = NotificationTrigger.POST_FINISHED_UPLOAD,
                    Receiver = pet.Owner,
                    IsRead = false
                };

                _notifications.Add(notification);
            }

            _notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                SentAt = DateTime.UtcNow,
                Message = $"Your post was succesfully uploaded!",
                Type = NotificationTrigger.POST_FINISHED_UPLOAD,
                Receiver = context.Creator,
                IsRead = false
            });
        }

        public async Task SendAsync(CancellationToken cancellationToken)
        {
            await _notificationService.CreateMany(cancellationToken, _notifications, _type);
        }
    }
}