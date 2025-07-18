using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid ReporterId { get; set; }
        public User Reporter { get; set; }
        public Guid ReportedId { get; set; }
        public string ReasonText { get; set; }
        public BanReason ReasonType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Moment>? Moments { get; set; } = new List<Moment>();
        public Report()
        {
        }
        //ctor 
        public Report(Guid id, Guid reporterId, User reporter, Guid reportedId, string reason, DateTime createdAt, string? description, List<Post> posts, List<Moment>? moments, BanReason reasonType)
        {
            Id = id;
            ReporterId = reporterId;
            Reporter = reporter;
            ReportedId = reportedId;
            ReasonType = reasonType;
            ReasonText = reason;
            CreatedAt = createdAt;
            Description = description;
            Posts = posts;
            Moments = moments ?? [];
        }
    }
}