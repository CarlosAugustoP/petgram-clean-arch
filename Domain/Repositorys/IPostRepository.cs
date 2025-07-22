using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedKernel.Common;
using Domain.Models;
using Domain.Models.UserAggregate;

namespace Domain.Repositorys
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken);
        Task<Post> UpdatePostAsync(Post post, CancellationToken cancellationToken);
        Task<Post> DeletePostAsync(Post post, CancellationToken cancellationToken);
        Task<Post?> GetPostByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Post>> GetPostsByUserAsync(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<PaginatedList<Pet>> GetPostsByPetIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Post>> GetPostByFollowingAsync(List<User> following, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<List<Post>> GetPostListByUserAsync(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
