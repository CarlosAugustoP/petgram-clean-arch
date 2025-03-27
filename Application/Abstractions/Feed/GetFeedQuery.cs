using MediatR;
using Domain.Models;
using Domain.Repositorys;
using SharedKernel.Common;

namespace Application.Abstractions.Feed
{
    public sealed record GetFeedQuery(Guid UserId, int PageIndex, int PageSize) : IRequest<PaginatedList<Post>>;
    internal sealed class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, PaginatedList<Post>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        public GetFeedQueryHandler(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        public async Task<PaginatedList<Post>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            var following = (await _userRepository.GetUserFollowingAsync(request.UserId, cancellationToken, request.PageIndex, request.PageSize)).Items;
            return await _postRepository.GetPostByFollowingAsync(
                following,
                request.PageIndex, request.PageSize, cancellationToken);
        }
    }
}
