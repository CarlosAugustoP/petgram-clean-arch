using Domain.Models;

namespace Domain.Repositorys
{
    public interface ICommentRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment> UpdateCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment> DeleteCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId, CancellationToken cancellationToken);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}