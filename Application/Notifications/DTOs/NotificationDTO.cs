using AutoMapper;
using Domain.Models.NotificationAggregate;

namespace Application.Notifications.DTOs
{
    [AutoMap(typeof(Notification))]

    public class NotificationDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public NotificationTrigger Type { get; set; }
        public bool IsRead { get; set; } = false;
        
        public NotificationDTO() { }

        public NotificationDTO(Guid id,string message, DateTime createdAt, NotificationTrigger type)
        {
            Id = id;
            Message = message;
            SentAt = createdAt;
            Type = type;
        }
    }
}
