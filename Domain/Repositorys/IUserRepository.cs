using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using SharedKernel.Common;
namespace Domain.Repositorys
{
    public interface IUserRepository
    {
       Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
       Task<User> AddUserToFollowers(User follower, User followed, CancellationToken cancellationToken);
       Task<User> CreateUser (User user, CancellationToken cancellationToken);
       Task<List<User>> GetAllUsers();
       Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
       Task<PaginatedList<User>> GetUserFollowingAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
       Task<PaginatedList<User>> GetUserFollowersAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
    }
}
