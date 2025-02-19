using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid ReporterId { get; set; }
        public User Reporter { get; set; }
        public Guid ReportedId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Moment>? Moments { get; set; } = new List<Moment>();
        public Report()
        {
        }
    }
}