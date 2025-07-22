using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositorys;
using FluentValidation;
using MediatR;
using SharedKernel.Common;

namespace Application.Abstractions.Reports
{
    public sealed record GetReportsByUserQuery(Guid UserId, int pageIndex, int pageSize) : IRequest<PaginatedList<Domain.Models.Report>>;
    internal sealed class GetReportsByUserQueryHandler : IRequestHandler<GetReportsByUserQuery, PaginatedList<Domain.Models.Report>>
    {
        private readonly IReportUserRepository _reportRepository;

        public GetReportsByUserQueryHandler(IReportUserRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<PaginatedList<Domain.Models.Report>> Handle(GetReportsByUserQuery request, CancellationToken cancellationToken)
        {
            return await _reportRepository.GetReportsByReportedIdAsync(request.UserId, cancellationToken, request.pageIndex, request.pageSize);
        }
    }
}