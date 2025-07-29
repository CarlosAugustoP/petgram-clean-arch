using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace API.Abstractions.Requests
{
    public class ReportUserRequest
    {
        public required string ReasonText { get; set; }
        public required BanReason ReasonType { get; set; }
        public required List<Guid> PostIds { get; set; }
        public required List<Guid> MomentIds { get; set; }
    }
}