using Domain.Models.UserAggregate;

namespace Application.UserManagement
{
    public interface IBanHammer
    {
        Task BanUserAsync(Guid userId, BanReason reason, UserBanOptions? options, CancellationToken cancellationToken);
        Task UnBanUserAsync(Guid userId, CancellationToken cancellationTokenbool, bool isSystemCall = false);
        Task<bool> IsUserBannedAsync(Guid userId, CancellationToken cancellationToken);
    }
}