using Domain.Models;

namespace Domain.Repositorys
{
    public interface IPetRepository
    {
        Task<Pet> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Pet>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken);
        Task<Pet> UpdateAsync(Pet pet, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Pet>> GetPetsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Pet>> GetPetsBySpeciesAsync(string Species, CancellationToken cancellationToken);
        Task<List<Pet>> GetPetsByUserPreferenceAsync(Preference preference, CancellationToken cancellationToken);
    }
}