using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Repositorys
{
    public interface IUserBanRepository
    {
        public Task<UserBan> GetByIdAsync(Guid id);
        public Task<UserBan> CreateBanAsync(UserBan userBan);
        public Task<UserBan> UpdateBanAsync(UserBan userBan);
    }
}