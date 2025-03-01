using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.LikeData
{
    public class LikeRepository : ILikeRepository
    {
        private readonly MainDBContext _db;
        public LikeRepository(MainDBContext db)
        {
            _db = db;
        }
        public async Task<Like> LikeCommentAsync(Like like, CancellationToken cancellationToken)
        {
            var likedComment = like.Comment!;
            like.Comment!.LikeCount++; 
            _db.Comments.Update(likedComment);
            await _db.AddAsync(like, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return like; 
        }
        public async Task<Like> LikePostAsync(Like like, CancellationToken cancellationToken)
        {
            var likedPost = like.Post!;
            like.Post!.LikesCount++;
            _db.Posts.Update(likedPost);
            await _db.AddAsync(like, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return like;
        }

        public async Task<Like> DislikePostAsync(Like like, CancellationToken cancellationToken)
        {
            var dislikedPost = like.Post!;
            like.Post!.LikesCount--;
            _db.Posts.Update(dislikedPost);
            _db.Likes.Remove(like);
            await _db.SaveChangesAsync(cancellationToken);
            return like;
        }

        public async Task<Like?> GetLikeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Likes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PaginatedList<Like>> GetLikesByPostIdAsync(Guid postId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Likes.Include(x => x.Author).Where(x => x.PostId == postId).AsQueryable();
            var list = await query.ToListAsync(cancellationToken);
            return await PaginatedList<Like>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<Like>> GetLikesByUserIdAsync(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Likes.Where(x=> x.AuthorId == userId).AsQueryable();
            return await PaginatedList<Like>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<Like> DislikeCommentAsync(Like like, CancellationToken cancellationToken)
        {
           var dislikedComment = like.Comment!;
           like.Comment!.LikeCount--;
           _db.Update(dislikedComment);
           _db.Likes.Remove(like);
           await _db.SaveChangesAsync(cancellationToken);
           return like;
        }
        public async Task<Like?> GetLikeByUserAndPostAsync(Guid userId, Guid postId, CancellationToken cancellationToken)
        {
            return await _db.Likes.FirstOrDefaultAsync(l => l.AuthorId == userId && l.PostId == postId, cancellationToken);
        }

        public Task<Like?> GetLikeByUserAndCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken)
        {
            return _db.Likes.FirstOrDefaultAsync(l => l.AuthorId == userId && l.CommentId == commentId, cancellationToken);
        }
    }
}