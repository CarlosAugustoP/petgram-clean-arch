using Domain.Models;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;

namespace Application.Abstractions.Posts.GetByUserQuery
{
    public sealed record GetByUserQuery(Guid UserId, int PageIndex = 0, int PageSize = 10) : IRequest<PaginatedList<Post>>;
    internal sealed class GetByUserQueryHandler : IRequestHandler<GetByUserQuery, PaginatedList<Post>>
    {
        private readonly IPostRepository _postRepository;

        public GetByUserQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PaginatedList<Post>> Handle(GetByUserQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetPostsByUserAsync(request.UserId, request.PageIndex, request.PageSize, cancellationToken);
        }
    }
}