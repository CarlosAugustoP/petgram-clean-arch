using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using Microsoft.AspNetCore.Diagnostics;

namespace Application.UserManagement
{
    public class BanHammer : IBanHammer
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserBanRepository _userBanRepository;
        public BanHammer(IUserRepository userRepository, IUserBanRepository userBanRepository)
        {
            _userBanRepository = userBanRepository;
            _userRepository = userRepository;
        }
        public async Task BanUserAsync(Guid userId, BanReason reason, UserBanOptions? options, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.BanUser();

            var userBan = new UserBan(
                Guid.NewGuid(),
                userId,
                user,
                reason,
                DateTime.UtcNow,
                options?.BanDuration.ToDateTime() ?? BanTimeEnum.ONEWEEK.ToDateTime(),
                options?.Remark
            );
            
            await _userBanRepository.CreateBanAsync(userBan);
            await _userRepository.UpdateUserAsync(user, cancellationToken);

        }

        public Task UnBanUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Implementation for unbanning a user
            throw new NotImplementedException();
        }

        public Task<bool> IsUserBannedAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Implementation to check if a user is banned
            throw new NotImplementedException();
        }
        
    }
}