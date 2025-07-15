using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Models
{
    public class Moment
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public Media Media { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Report>? Reports {get; set;} = new List<Report>();
        public int Shares { get; set; }
        public Moment()
        {
            
        }


    }
}
