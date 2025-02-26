using Domain.Models;

namespace Domain.Repositorys
{
    public interface ICommentRepository
    {
        Task<Comment> CreateComment(Comment comment, CancellationToken cancellationToken);
        Task<Comment> UpdateComment(Comment comment, CancellationToken cancellationToken);
        Task<Comment> DeleteComment(Comment comment, CancellationToken cancellationToken);
        Task<Comment> GetCommentById(Guid id, CancellationToken cancellationToken);
        Task<List<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken);
        Task<List<Comment>> GetCommentsByUserId(Guid userId, CancellationToken cancellationToken);
    }
}