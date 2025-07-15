using Domain.CustomExceptions;
using Domain.Models.UserAggregate;
using Domain.Repositorys;


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

            await _userBanRepository.CreateBanAsync(userBan, cancellationToken);
            await _userRepository.UpdateUserAsync(user, cancellationToken);

        }

        public async Task UnBanUserAsync(Guid userId, CancellationToken cancellationToken, bool isSystemCall = false)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.InactiveUser();
            var userBan = await _userBanRepository.GetByIdAsync(userId, cancellationToken);

            if (isSystemCall) userBan.ExpireBan();
            else userBan.RevokeBan();

            await _userRepository.UpdateUserAsync(user, cancellationToken);
            await _userBanRepository.UpdateBanAsync(userBan, cancellationToken);
        }

        public Task<bool> IsUserBannedAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _userRepository.GetByIdAsync(userId, cancellationToken)
                .ContinueWith(task =>
                {
                    var user = task.Result;
                    return user != null && user.IsBanned();
                }, cancellationToken);
        }
        
    }
}