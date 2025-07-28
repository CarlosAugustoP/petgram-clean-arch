using Application.Notifications;
using Application.Notifications.Implementations;
using Domain.CustomExceptions;
using Domain.Models.NotificationAggregate;
using Domain.Repositorys;
using MediatR;
using static Application.Notifications.Implementations.OnFollowUser;

namespace Application.Abstractions.Followers.StartFollowing
{
    public sealed record StartFollowingCommand : IRequest<object>
    {
        public Guid FollowerId { get; init; }
        public Guid FollowedId { get; init; }
    }
    internal sealed class StartFollowingCommandHandler : IRequestHandler<StartFollowingCommand, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly NotificationFactory _notificationFactory;

        public StartFollowingCommandHandler(IUserRepository userRepository, NotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
            _userRepository = userRepository;
        }

        public async Task<object> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
        {
            var follower = await _userRepository.GetByIdAsync(command.FollowerId, cancellationToken)
                ?? throw new NotFoundException("Follower not found!");
            
            var followed = await _userRepository.GetByIdAsync(command.FollowedId, cancellationToken)
                ?? throw new NotFoundException("Followed not found!");
            
            if (followed.Id == follower.Id)
                throw new BadRequestException("You can't follow yourself!");
            
            var unfollower = await _userRepository.IsFollowingAsync(follower.Id, followed, cancellationToken);
            
            if (unfollower != null){
                await _userRepository.RemoveUserFromFollowersAsync(unfollower, followed, cancellationToken);
                return followed;
            }
            
            await _userRepository.AddUserToFollowersAsync(follower, followed, cancellationToken);

            await _notificationFactory.Create(NotificationTrigger.NEW_FOLLOWER).ExecuteAsync(new OnFollowUserContext(follower, followed.Id), cancellationToken);

            return followed;
        }
    }
}
