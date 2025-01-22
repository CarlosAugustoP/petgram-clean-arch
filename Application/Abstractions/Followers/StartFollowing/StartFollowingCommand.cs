using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Application.Abstractions.Messaging;
using Application.Abstractions.Returns;
using Domain.Models;
using Domain.Services;

namespace Application.Abstractions.Followers.StartFollowing
{
    public sealed record StartFollowingCommand : Messaging.ICommand
    {
        public Guid FollowerId { get; init; }
        public Guid FollowedId { get; init; }
    }
    internal sealed class StartFollowingCommandHandler : ICommandHandler<StartFollowingCommand>
    {
        private readonly UserService _userService;

        public StartFollowingCommandHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<object>> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
        {
            var user = await _userService.UserFollowUser(command.FollowerId, command.FollowedId, cancellationToken);
            return Result<object>.Success(user);
        }
    }  
}
