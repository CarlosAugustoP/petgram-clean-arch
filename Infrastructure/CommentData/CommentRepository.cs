using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

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

            await _db.Comments.AddAsync(comment, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return comment;
        }

        public async Task<Comment> CreateReplyAsync(Comment comment, CancellationToken cancellationToken)
        {
            comment.BaseComment!.RepliesCount++;
            await _db.Comments.AddAsync(comment, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return comment;
        }


        public async Task<Comment> DeleteCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync(cancellationToken);
            return comment;
        }

        public async Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Comments.FindAsync(id, cancellationToken);
        }

        public Task<PaginatedList<Comment>> GetCommentsByPostIdAsync(Guid postId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Comments.Include(x => x.Author).Where(c => c.PostId == postId).AsQueryable();
            return PaginatedList<Comment>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment, string content, CancellationToken cancellationToken)
        {
            comment.Content = content;
            comment.IsEdited = true; 
            _db.Comments.Update(comment);
            await _db.SaveChangesAsync(cancellationToken);
            return comment;
        }
    }
}