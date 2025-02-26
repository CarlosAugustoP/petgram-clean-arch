using Domain.Models;
using SharedKernel.Common;

namespace Domain.Repositorys
{
    public interface ILikeRepository
    {
        Task<Like> LikeComment(Like like, CancellationToken cancellationToken);
        Task<Like> LikePost(Like like, CancellationToken cancellationToken);
        Task<Like> DislikePost(Like like, CancellationToken cancellationToken);
        Task<Like> DislikeComment(Like like, CancellationToken cancellationToken);
        Task<Like?> GetLikeById(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Like>> GetLikesByPostId(Guid postId, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<PaginatedList<Like>> GetLikesByUserId(Guid userId,  int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<Like?> GetLikeByUserAndPost(Guid userId, Guid postId, CancellationToken cancellationToken);

    }
}