﻿using System;
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
        public async Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken)
        {
            await _db.Posts.AddAsync(post, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);   
            return post;
        }

        public async Task<Post> DeletePostAsync(Post post, CancellationToken cancellationToken)
        {
            _db.Remove(post);
            await _db.SaveChangesAsync(cancellationToken);
            return post!;
        }

        public async Task<Post?> GetPostByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Posts.Include(x => x.Medias).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> UpdatePostAsync(Post post, CancellationToken cancellationToken)
        {
            _db.Posts.Update(post);
            await _db.SaveChangesAsync(cancellationToken);
            return post;
        }

        public Task<PaginatedList<Pet>> GetPostsByPetIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        public Task<PaginatedList<Post>> GetPostsByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Post>> GetPostsByUserPreferenceAsync(Preference preference, CancellationToken cancellationToken, List<Post> posts)
        {
            throw new NotImplementedException();
        }
    }
}
