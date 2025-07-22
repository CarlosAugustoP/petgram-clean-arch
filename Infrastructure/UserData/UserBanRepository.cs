using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserData
{
    public class UserBanRepository : IUserBanRepository
    {
        private readonly MainDBContext _db;
        public UserBanRepository(MainDBContext db)
        {
            _db = db;
        }
        public async Task<UserBan> CreateBanAsync(UserBan userBan, CancellationToken cancellationToken)
        {
            _db.UserBans.Add(userBan);
            await _db.SaveChangesAsync(cancellationToken);
            return userBan;
        }

        public async Task<UserBan?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.UserBans.FirstOrDefaultAsync(ub => ub.Id == id, cancellationToken);
        }

        public Task<List<UserBan>> GetExpiredBansAsync(CancellationToken cancellationToken)
        {
            return _db.UserBans
                .Where(ub => ub.ToBeUnbannedAt < DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserBan> UpdateBanAsync(UserBan userBan, CancellationToken cancellationToken)
        {
            _db.UserBans.Update(userBan);
            await _db.SaveChangesAsync(cancellationToken);
            return userBan;
        }
    }
}