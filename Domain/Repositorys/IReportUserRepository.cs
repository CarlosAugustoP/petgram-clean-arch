using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using SharedKernel.Common;

namespace Domain.Repositorys
{
    public interface IReportUserRepository
    {
        public Task<Report?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<Report>> GetAllAsync(CancellationToken cancellationToken);
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        public Task<Report> CreateAsync(Report report, CancellationToken cancellationToken);
        public Task<Report> UpdateAsync(Report report, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<Report>> GetReportsByReporterIdAsync(Guid reporterId, CancellationToken cancellationToken);
        public Task<PaginatedList<Report>> GetReportsByReportedIdAsync(Guid reportedId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
        
    }
}