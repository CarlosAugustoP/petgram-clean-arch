using Domain.Models;
using SharedKernel.Common;

namespace Domain.Repositorys
{
    public interface ICommentRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment> UpdateCommentAsync(Comment comment, string content, CancellationToken cancellationToken);
        Task<Comment> DeleteCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Comment>> GetCommentsByPostIdAsync(Guid postId, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Comment> CreateReplyAsync(Comment comment, CancellationToken cancellationToken);

        
    }
}