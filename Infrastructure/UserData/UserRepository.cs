using SharedKernel.Common;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Domain.Models.UserAggregate;

namespace Infrastructure.UserData
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDBContext _db;
        public UserRepository(MainDBContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != UserStatus.DELETED && u.Status != UserStatus.ARCHIVED, cancellationToken);
        }

        public async Task<PaginatedList<User>> GetUserFollowersAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
            var followersQuery = _db.Users
                .Where(u => u.Id == userId && u.Status != UserStatus.DELETED && u.Status != UserStatus.ARCHIVED)
                .Include(u => u.Followers)
                .SelectMany(u => u.Followers!)
                .AsQueryable();

            return await PaginatedList<User>.CreateAsync(followersQuery, pageIndex, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<User>> GetUserFollowingAsync(Guid userId, CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
            var followingQuery = _db.Users
                .Where(u => u.Id == userId && u.Status != UserStatus.DELETED && u.Status != UserStatus.ARCHIVED)
                .Include(u => u.Following)
                .SelectMany(u => u.Following!)
                .AsQueryable();
            return await PaginatedList<User>.CreateAsync(followingQuery, pageIndex, pageSize, cancellationToken);
        }


        public async Task<User> AddUserToFollowersAsync(User follower, User followed, CancellationToken cancellationToken)
        {
            followed.Followers!.Add(follower);
            follower.Following!.Add(followed);
            await _db.SaveChangesAsync(cancellationToken);
            return followed;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _db.Users.Where(x => !x.Status.Equals(UserStatus.DELETED)).ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            await _db.Users.AddAsync(user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status != UserStatus.DELETED && u.Status != UserStatus.ARCHIVED, cancellationToken);
        }

        public async Task<bool> RemoveUserFromFollowersAsync(User unfollower, User unfollowed, CancellationToken cancellationToken)
        {
            unfollower.Following!.Remove(unfollowed);
            unfollowed.Followers!.Remove(unfollower);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<User> IsFollowingAsync(Guid userId, User followed, CancellationToken cancellationToken)
        {
            var follower = await _db.Users
                .Include(u => u.Following)
                .Where(u => u.Status != UserStatus.DELETED && u.Status != UserStatus.ARCHIVED)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (followed.Followers!.Contains(follower!))
            {
                return follower!;
            }
            return null!;
        }

        public Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            _db.Users.Update(user);
            return _db.SaveChangesAsync(cancellationToken);
        }

        public Task<List<User>> GetInactiveUsersAsync(CancellationToken cancellationToken = default)
        {
            return _db.Users
                .Where(u => u.Status == UserStatus.INACTIVE)
                .ToListAsync(cancellationToken);
        }
        
        /// <summary>
        /// For admin. Retrieves all except deleted users.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IQueryable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_db.Users.Include(u => u.Followers).Include(u => u.Following).Where(x => x.Status != UserStatus.DELETED).AsQueryable());
        }
    }
}
