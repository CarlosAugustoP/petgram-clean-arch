using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Users Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool isEdited { get; set; }
        public List<Comment> Replies { get; set; }
        public Comment()
        {
        }
    }
}
