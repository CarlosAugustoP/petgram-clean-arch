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
        
        public async Task<PaginatedList<Post>> GetPostsByUserAsync(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _db.Posts.Where(x => x.AuthorId == userId).AsQueryable();
            return await PaginatedList<Post>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
        }

        
        public async Task<List<Post>> GetPostListByUserAsync(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {            
            var query = _db.Posts.Where(x => x.AuthorId == userId).Include(x => x.Medias).AsQueryable();
            var list = (await PaginatedList<Post>.CreateAsync(
                query, pageIndex, pageSize, cancellationToken
            )).Items.ToList();
            Console.WriteLine("List Count: " + list.Count);
            return list;
        }

        public Task<PaginatedList<Post>> GetPostsByUserPreferenceAsync(Preference preference, CancellationToken cancellationToken, List<Post> posts)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedList<Post>> GetPostByFollowingAsync(List<User> following, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var list = new List<Post>();
            if (following.Count == 0)
            {
                throw new Exception();
            }
           
            var postTasks = following
                .Select(f => GetPostListByUserAsync(f.Id, pageIndex, pageSize, cancellationToken));
            //await the result
            var postLists = await Task.WhenAll(postTasks);
            //adds each selected post each post list
            list.AddRange(postLists.SelectMany(posts => posts));
            
            return new PaginatedList<Post>(list,list.Count,pageIndex,pageSize);
        }
    }
}
