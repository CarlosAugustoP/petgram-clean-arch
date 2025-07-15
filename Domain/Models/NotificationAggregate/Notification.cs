using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Models.NotificationAggregate
{
    public class Notification
    {
        public Guid Id { get; set; }
        public DateTime SentAt { get; set; }
        public string Message { get; set; }
        public NotificationTrigger Type { get; set; }
        public Guid? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public bool IsRead { get; set; } = false;
        public Notification() { }

        public Notification(Guid id, DateTime sentAt, string message, NotificationTrigger type, Guid receiverId, User? receiver)
        {
            Id = id;
            SentAt = sentAt;
            Message = message;
            Type = type;
            Receiver = receiver;
            ReceiverId = receiverId;
        }

    }
}
