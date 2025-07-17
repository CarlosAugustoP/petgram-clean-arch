using Domain.CustomExceptions;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Users.GetProfile
{
    public record GetProfileQuery(Guid UserId) : IRequest<(User, int, int)>;
    internal sealed class GetProfileQueryHandler(IUserRepository userRepository) : IRequestHandler<GetProfileQuery, (User, int, int)>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<(User, int, int)> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found.");

            var followed = await _userRepository.GetUserFollowingAsync(request.UserId, cancellationToken);
            var followers = await _userRepository.GetUserFollowersAsync(request.UserId, cancellationToken);

            var followedCount = followed.TotalCount;
            var followersCount = followers.TotalCount;

            return (user, followedCount, followersCount);
        }
    }
}