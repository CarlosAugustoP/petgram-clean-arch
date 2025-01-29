using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public DateTime SentAt { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
        public bool IsRead { get; set; }
        public Notification() { }

    }
}
