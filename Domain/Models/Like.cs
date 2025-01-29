using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Like
    {
    public Like() { }
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
