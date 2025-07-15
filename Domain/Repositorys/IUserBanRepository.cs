using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Repositorys
{
    public interface IUserBanRepository
    {
        public Task<UserBan?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UserBan> CreateBanAsync(UserBan userBan, CancellationToken cancellationToken);
        public Task<UserBan> UpdateBanAsync(UserBan userBan, CancellationToken cancellationToken);
    }
}