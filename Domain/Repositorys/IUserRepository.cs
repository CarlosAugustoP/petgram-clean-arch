using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.UserAggregate;
using SharedKernel.Common;
namespace Domain.Repositorys
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User> AddUserToFollowersAsync(User follower, User followed, CancellationToken cancellationToken);
        Task<bool> RemoveUserFromFollowersAsync(User unfollower, User unfollowed, CancellationToken cancellationToken);
        Task<User> IsFollowingAsync(Guid followerId, User followed, CancellationToken cancellationToken);
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<PaginatedList<User>> GetUserFollowingAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
        Task<PaginatedList<User>> GetUserFollowersAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<List<User>> GetInactiveUsersAsync(CancellationToken cancellationToken = default);
        Task<IQueryable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    }
}
