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
       Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
       Task<User> AddUserToFollowers(User follower, User followed, CancellationToken cancellationToken = default);
       Task<User> CreateUser (User user, CancellationToken cancellationToken = default);
       Task<List<User>> GetAllUsers();
       Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default);
       Task<PaginatedList<User>> GetUserFollowingAsync(Guid userId, int pageIndex = 1, int pageSize = 10);
       Task<PaginatedList<User>> GetUserFollowersAsync(Guid userId, int pageIndex = 1, int pageSize = 10);
    }
}
