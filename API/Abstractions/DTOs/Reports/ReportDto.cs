using AutoMapper;
using Domain.Models.UserAggregate;

namespace API.Abstractions.DTOs.Reports
{
    [AutoMap(typeof(Domain.Models.Report))]
    public class ReportDto
    {
        public Guid Id { get; set; }
        public Guid ReporterId { get; set; }
        public Guid ReportedId { get; set; }
        public required string ReasonText { get; set; }
        public BanReason ReasonType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
    }
}