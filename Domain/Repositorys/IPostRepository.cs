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
        Task<Post> CreatePost(Post post, CancellationToken cancellationToken);
        Task<Post> UpdatePost(Post post, CancellationToken cancellationToken);
        Task<Post> DeletePost(Post post, CancellationToken cancellationToken);
        Task<Post?> GetPostById(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<Post>> GetPostsByUserPreference(Preference preference, CancellationToken cancellationToken);
        Task<PaginatedList<Post>> GetPostsByUser(Guid userId, CancellationToken cancellationToken);
        Task<PaginatedList<Pet>>GetPostsByPetId(Guid id, CancellationToken cancellationToken); 
       
    }
}
