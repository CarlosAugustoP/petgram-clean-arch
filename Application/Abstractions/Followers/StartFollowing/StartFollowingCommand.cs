using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Application.Abstractions.Messaging;
using Application.Abstractions.Returns;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.UserData;

namespace Application.Abstractions.Followers.StartFollowing
{
    public sealed record StartFollowingCommand : Messaging.ICommand
    {
        public Guid FollowerId { get; init; }
        public Guid FollowedId { get; init; }
    }
    internal sealed class StartFollowingCommandHandler : ICommandHandler<StartFollowingCommand>
    {
        private readonly IUserRepository _userRepository;

        public StartFollowingCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<object>> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
        {
            var follower = await _userRepository.GetByIdAsync(command.FollowerId, cancellationToken);
            if (follower == null) {
                throw new NotFoundException("Follower not found!");
            }
            var followed = await _userRepository.GetByIdAsync(command.FollowedId, cancellationToken);
            if (followed == null)
            {
                throw new NotFoundException("Followed not found!");
            }
            return Result<object>.Success(await _userRepository.AddUserToFollowers(follower, followed, cancellationToken));
        }
    }
}
