using Domain.Models;

namespace Domain.Repositorys
{
    public interface IMomentRepository
    {
        Task<Moment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Moment>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Moment> CreateAsync(Moment moment, CancellationToken cancellationToken);
        Task<Moment> UpdateAsync(Moment moment, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Moment>> GetMomentsByUserIdAsync(Guid userId, CancellationToken cancellationToken);  
    }
}