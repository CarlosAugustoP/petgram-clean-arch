using AutoMapper;
using Domain.Models.Notification;

namespace Application.Notifications.DTOs
{
    [AutoMap(typeof(Notification))]

    public class NotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; } = false;

        public NotificationDTO(string title, string message, DateTime createdAt)
        {
            Title = title;
            Message = message;
            CreatedAt = createdAt;
        }
    }
}
