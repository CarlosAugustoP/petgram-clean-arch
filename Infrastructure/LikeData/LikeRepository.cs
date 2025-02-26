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
        public async Task<Like> LikeComment(Like like, CancellationToken cancellationToken)
        {
            var likedComment = like.Comment!;
            like.Comment!.LikeCount++;
            _db.Comments.Update(likedComment);
            await _db.AddAsync(like, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return like; 
        }
        public async Task<Like> LikePost(Like like, CancellationToken cancellationToken)
        {
            var likedPost = like.Post!;
            like.Post!.LikesCount++;
            _db.Posts.Update(likedPost);
            await _db.AddAsync(like, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return like;
        }

        public async Task<Like> DislikePost(Like like, CancellationToken cancellationToken)
        {
            var dislikedPost = like.Post!;
            like.Post!.LikesCount--;
            _db.Posts.Update(dislikedPost);
            _db.Likes.Remove(like);
            await _db.SaveChangesAsync(cancellationToken);
            return like;
        }

        public async Task<Like?> GetLikeById(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Likes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PaginatedList<Like>> GetLikesByPostId(Guid postId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Likes.Where(x => x.PostId == postId).AsQueryable();
            return await PaginatedList<Like>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<Like>> GetLikesByUserId(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Likes.Where(x=> x.AuthorId == userId).AsQueryable();
            return await PaginatedList<Like>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        public async Task<Like> DislikeComment(Like like, CancellationToken cancellationToken)
        {
           var dislikedComment = like.Comment!;
           like.Comment!.LikeCount--;
           _db.Update(dislikedComment);
           _db.Likes.Remove(like);
           await _db.SaveChangesAsync(cancellationToken);
           return like;
        }

        public async Task<bool> HasLiked(Guid userId, Guid postId, CancellationToken cancellationToken)
        {
            return await _db.Likes.AnyAsync(l => l.AuthorId == userId && l.PostId == postId, cancellationToken);
        }

        public async Task<Like?> GetLikeByUserAndPost(Guid userId, Guid postId, CancellationToken cancellationToken)
        {
            return await _db.Likes.FirstOrDefaultAsync(l => l.AuthorId == userId && l.PostId == postId, cancellationToken);
        }

        
    }
}