using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;
using Domain.Models;
namespace Application.Abstractions.Likes.GetLikesByPostQuery
{
    public sealed record GetLikesByPostQuery : IRequest<PaginatedList<Domain.Models.Like>>
    {
        public Guid PostId { get; init; }
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public GetLikesByPostQuery(Guid postId, int pageIndex, int pageSize)
        {
            PostId = postId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }

    internal sealed class GetLikesByPostQueryHandler : IRequestHandler<GetLikesByPostQuery, PaginatedList<Like>>
    {
        private readonly ILikeRepository _likeRepository;

        public GetLikesByPostQueryHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task<PaginatedList<Like>> Handle(GetLikesByPostQuery request, CancellationToken cancellationToken)
        {
            var likes = await _likeRepository.GetLikesByPostIdAsync(request.PostId, request.PageIndex, request.PageSize, cancellationToken);
            return likes;
        }
    }
}