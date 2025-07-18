// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Domain.Models;
// using Domain.Models.UserAggregate;
// using MediatR;

// namespace Application.Abstractions.Report
// {
//     public sealed record ReportUserCommand
//         (Guid ReporterId, Guid ReportedId, string ReasonText, BanReason ReasonType, List<Guid> PostIds, List<Guid>? MomentIds)
//      : IRequest<Domain.Models.Report>;

//     internal sealed class ReportUserCommandHandler : IRequestHandler<ReportUserCommand, Domain.Models.Report>
//     {
//         private readonly IReportRepository _reportRepository;
//         private readonly IUserRepository _userRepository;

//         public ReportUserCommandHandler(IReportRepository reportRepository, IUserRepository userRepository)
//         {
//             _reportRepository = reportRepository;
//             _userRepository = userRepository;
//         }

//         Task<Domain.Models.Report> IRequestHandler<ReportUserCommand, Domain.Models.Report>.Handle(ReportUserCommand request, CancellationToken cancellationToken)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }