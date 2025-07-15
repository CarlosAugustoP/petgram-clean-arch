using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;

namespace Application.Abstractions.Followers.GetFollowers
{
    public sealed record GetFollowersByUserQuery : IRequest<PaginatedList<User>>
    {
        public Guid UserId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    internal sealed class GetFollowersByUserQueryHandler : IRequestHandler<GetFollowersByUserQuery, PaginatedList<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowersByUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<User>> Handle(GetFollowersByUserQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserFollowersAsync(query.UserId, cancellationToken, query.PageIndex, query.PageSize);
        }
    }
}