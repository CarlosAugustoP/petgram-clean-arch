using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Notification
    {
        public DateTime sentAt { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public Users? sender { get; set; }
        public Users? receiver { get; set; }
        public bool isRead { get; set; }
        public Notification() { }

    }
}
