using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Followers.GetFollowers
{
    public sealed record GetFollowersByUserQuery : IRequest<List<User>>
    {
        public Guid UserId { get; set; }
    }

    internal sealed class GetFollowersByUserQueryHandler : IRequestHandler<GetFollowersByUserQuery, List<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowersByUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetFollowersByUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found!");

            return user.Followers;
        }
    }
}