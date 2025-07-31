using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Users.ArchiveOrDeleteProfile
{
    public sealed record ConfirmActionCommand(Guid UserId, string Token) : IRequest<bool>;

    internal sealed class ConfirmActionCommandHandler : IRequestHandler<ConfirmActionCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisService _redisService;

        public ConfirmActionCommandHandler(IUserRepository userRepository, IRedisService redisService)
        {
            _userRepository = userRepository;
            _redisService = redisService;
        }

        public async Task<bool> Handle(ConfirmActionCommand request, CancellationToken cancellationToken)
        {
            var redisKey = $"profile-action:{request.UserId}";
            var tokenData = await _redisService.GetObjectAsync<TokenData>(redisKey);

            if (tokenData == null || tokenData.Token != request.Token)
            {
                throw new ForbiddenException("Invalid or expired token");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            if (tokenData.Status == UserStatus.ARCHIVED)
            {
                user.ArchiveUser();
            }
            else if (tokenData.Status == UserStatus.DELETED)
            {
                user.DeleteUser();
            }
            else
            {
                throw new InvalidOperationException("Invalid status in token data");
            }
            
            await _userRepository.UpdateUserAsync(user, cancellationToken);
            await _redisService.DeleteAsync(redisKey);

            return true; 
        }
    }
}