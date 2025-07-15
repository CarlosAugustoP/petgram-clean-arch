using Application.UserManagement;
using Domain.Models.UserAggregate;
using MediatR;

namespace Application.Abstractions.Users.BanUser
{
    public sealed record BanUserCommand(Guid UserId, BanReason Reason, string? Remark, BanTimeEnum? BanExpiration) : IRequest<bool>;

    internal sealed class BanUserCommandHandler : IRequestHandler<BanUserCommand, bool>
    {
        private readonly IBanHammer _banHammer;

        public BanUserCommandHandler(IBanHammer banHammer)
        {
            _banHammer = banHammer;
        }

        public async Task<bool> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            await _banHammer.BanUserAsync(
                request.UserId,
                request.Reason,
                new UserBanOptions
                {
                    Remark = request.Remark,
                    BanDuration = request.BanExpiration ?? BanTimeEnum.ONEWEEK
                },
                cancellationToken
            );
            return true;
        }
    } 
}