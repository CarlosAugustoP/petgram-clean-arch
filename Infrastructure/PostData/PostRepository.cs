using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.PostData
{
    public class PostRepository : IPostRepository
    {
        private readonly MainDBContext _db;

        public PostRepository(MainDBContext db)
        {
            _db = db;
        }
        public async Task<Post> CreatePost(Post post)
        {
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();   
            return post;
        }

        public async Task<Post> DeletePost(Post post)
        {
            _db.Remove(post);
            await _db.SaveChangesAsync();
            return post!;
        }

        public async Task<Post?> GetPostById(Guid id)
        {
            return await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<PaginatedList<Pet>> GetPostsByPetId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Post>> GetPostsByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Post>> GetPostsByUserPreference(Preference preference)
        {
            throw new NotImplementedException();
        }

        public Task<Post> UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
