using Domain.Models;
using SharedKernel.Common;

namespace Domain.Repositorys
{
    public interface ILikeRepository
    {
        Task<Like> LikeCommentAsync(Like like, CancellationToken cancellationToken);
        Task<Like> LikePostAsync(Like like, CancellationToken cancellationToken);
        Task<Like> DislikePostAsync(Like like, CancellationToken cancellationToken);
        Task<Like> DislikeCommentAsync(Like like, CancellationToken cancellationToken);
        Task<Like?> GetLikeByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Like>> GetLikesByPostIdAsync(Guid postId, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<PaginatedList<Like>> GetLikesByUserIdAsync(Guid userId,  int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<Like?> GetLikeByUserAndPostAsync(Guid userId, Guid postId, CancellationToken cancellationToken);
        Task<Like?> GetLikeByUserAndCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken);

    }
}