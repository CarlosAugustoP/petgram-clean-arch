using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Followers.GetFollowingByUser
{
    public class GetFollowingByUserQuery : IRequest<List<User>>
    {
        public Guid UserId { get; set; }
    }

    public class GetFollowingByUserQueryHandler : IRequestHandler<GetFollowingByUserQuery, List<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowingByUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetFollowingByUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found!");

            return user.Following;
        }
    }
}