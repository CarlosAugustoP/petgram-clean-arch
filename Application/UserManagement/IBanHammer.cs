using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Application.UserManagement
{
    public interface IBanHammer
    {
        Task BanUserAsync(Guid userId, BanReason reason, UserBanOptions? options, CancellationToken cancellationToken);
        Task UnBanUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsUserBannedAsync(Guid userId, CancellationToken cancellationToken);
    }
}