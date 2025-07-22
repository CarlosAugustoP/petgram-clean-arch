using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using SharedKernel.Common;

namespace Infrastructure.UserData
{
    public class ReportUserRepository : IReportUserRepository
    {
        public Task<Report> CreateAsync(Report report, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Report>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Report?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Report>> GetReportsByReportedIdAsync(Guid reportedId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<List<Report>> GetReportsByReporterIdAsync(Guid reporterId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Report> UpdateAsync(Report report, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}