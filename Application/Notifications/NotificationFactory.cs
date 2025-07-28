using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Notifications.Implementations;
using Domain.Models.NotificationAggregate;

namespace Application.Notifications
{
    public class NotificationFactory
    {
        private readonly NotificationService _notificationService;

        public NotificationFactory(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public INotification Create(NotificationTrigger type)
        {
            return type switch
            {
                NotificationTrigger.POST_FINISHED_UPLOAD => new OnSuccessfulUploadPost(_notificationService),
                NotificationTrigger.NEW_USER => new OnNewUser(_notificationService),
                NotificationTrigger.INACTIVE_USER_REMINDER => new OnInactiveUserReminded(_notificationService),
                // NotificationTrigger.LIKED_POST => new OnLikePost(_notificationService),
                // NotificationTrigger.COMMENTED_POST => new OnCommentPost(_notificationService),
                NotificationTrigger.NEW_FOLLOWER => new OnFollowUser(_notificationService),
                NotificationTrigger.NEW_MESSAGE => new OnNewMessage(_notificationService),
                
                _ => throw new ArgumentException($"Notification type {type} is not supported.")
                
            };
        }
    }
}