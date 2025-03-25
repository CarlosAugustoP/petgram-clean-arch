using Domain.Models;
using Domain.Repositorys;

namespace Infrastructure.PetData 
{
    public class PetRepository : IPetRepository
    {
        public Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pet>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Pet> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pet>> GetPetsBySpeciesAsync(string Species, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pet>> GetPetsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pet>> GetPetsByUserPreferenceAsync(Preference preference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Pet> UpdateAsync(Pet pet, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}