using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;

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

        public StartFollowingCommandHandler(IUserRepository userRepository)
        {
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
            return followed;
        }
    }
}
