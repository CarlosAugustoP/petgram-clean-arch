using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Media
    {
        public Guid Id { get; set; }
        public Post post { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public Media() { }

    }
}
