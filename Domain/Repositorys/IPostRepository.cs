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
        Task<Post> CreatePost(Post post);
        Task<Post> UpdatePost(Post post);
        Task<Post> DeletePost(Post post);
        Task<Post?> GetPostById(Guid id);
        Task<PaginatedList<Post>> GetPostsByUserPreference(Preference preference);
        Task<PaginatedList<Post>> GetPostsByUser(Guid userId);
        Task<PaginatedList<Pet>>GetPostsByPetId(Guid id);
       
    }
}
