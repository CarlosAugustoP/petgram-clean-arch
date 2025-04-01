using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PetData 
{
    public class PetRepository : IPetRepository
    {
        private readonly MainDBContext _db;
        
        public PetRepository(MainDBContext db)
        {
            _db = db;
        }

        public async Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken)
        {
            _db.Pets.Add(pet);
            await _db.SaveChangesAsync(cancellationToken);
            return pet;
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return _db.Pets.AnyAsync(p => p.Id == id, cancellationToken);
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