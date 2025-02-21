using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;
namespace Application.Abstractions.Followers.GetFollowingByUser
{
    public sealed record GetFollowingByUserQuery : IRequest<PaginatedList<User>>
    {
        public Guid UserId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    internal sealed class GetFollowingByUserQueryHandler : IRequestHandler<GetFollowingByUserQuery, PaginatedList<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowingByUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<User>> Handle(GetFollowingByUserQuery query, CancellationToken cancellationToken)
        {
           return await _userRepository.GetUserFollowingAsync(query.UserId, cancellationToken, query.PageIndex, query.PageSize);
        }
    }
}