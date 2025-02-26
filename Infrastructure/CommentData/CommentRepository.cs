using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;

namespace Infrastructure.CommentData
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MainDBContext _db;
        public CommentRepository(MainDBContext db)
        {
            _db = db;
        }
        public async Task<Comment> CreateCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
            return comment;
        }

        public Task<Comment> DeleteCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> UpdateCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}