using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedKernel.Common;
using Domain.Models;

namespace Domain.Repositorys
{
    public interface IPostRepository
    {
        Task<Post> CreatePost(Post post, CancellationToken cancellationToken = default);
        Task<Post> UpdatePost(Post post, CancellationToken cancellationToken = default);
        Task<Post> DeletePost(Post post, CancellationToken cancellationToken = default);
        Task<Post?> GetPostById(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedList<Post>> GetPostsByUserPreference(Preference preference, CancellationToken cancellationToken = default);
        Task<PaginatedList<Post>> GetPostsByUser(Guid userId, CancellationToken cancellationToken = default);
        Task<PaginatedList<Pet>>GetPostsByPetId(Guid id, CancellationToken cancellationToken = default);   
       
    }
}
