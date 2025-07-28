using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MomentData
{
    public class MomentRepository(MainDBContext db) : IMomentRepository
    {
        private readonly MainDBContext _db = db;

        public Task<Moment> CreateAsync(Moment moment, CancellationToken cancellationToken)
        {
            _db.Moments.Add(moment);
            return Task.FromResult(moment);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var moment = await GetQueryable().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (moment != null)
            {
                _db.Moments.Remove(moment);
            }
            await _db.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetQueryable().Any(m => m.Id == id));
        }

        public Task<List<Moment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetQueryable().ToList());
        }

        public async Task<Moment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetQueryable().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<List<Moment>> GetMomentsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await GetQueryable().Where(m => m.AuthorId == userId).ToListAsync(cancellationToken);
        }

        public async Task<Moment> UpdateAsync(Moment moment, CancellationToken cancellationToken)
        {
            _db.Moments.Update(moment);
            await _db.SaveChangesAsync(cancellationToken);
            return moment;
        }

        public IQueryable<Moment> GetQueryable()
        {
            //like instagram stories moments expire in 24 hours
            return _db.Moments.Include(x => x.Media).Where(x => x.CreatedAt.AddHours(24) > DateTime.UtcNow).AsQueryable();
        }
    }
}