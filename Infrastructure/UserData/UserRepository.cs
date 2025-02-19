using SharedKernel.Common;
using Domain.Models;
using Domain.Repositorys;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserData
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDBContext _db;
        public UserRepository(MainDBContext db) {
            _db = db;
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<PaginatedList<User>> GetUserFollowersAsync(Guid userId, int pageIndex = 1, int pageSize = 10)
        {
            var followersQuery = _db.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Followers)
                .SelectMany(u => u.Followers)
                .AsQueryable();

            return await PaginatedList<User>.CreateAsync(followersQuery, pageIndex, pageSize);
        }

        public async Task<PaginatedList<User>> GetUserFollowingAsync(Guid userId, int pageIndex = 1, int pageSize = 10)
        {
            var followingQuery = _db.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Following)
                .SelectMany(u => u.Following)
                .AsQueryable();
            return await PaginatedList<User>.CreateAsync(followingQuery, pageIndex, pageSize);
        }


        public async Task<User> AddUserToFollowers(User follower, User followed, CancellationToken cancellationToken)
        {
            followed.Followers.Add(follower);
            follower.Following.Add(followed);
            await _db.SaveChangesAsync(cancellationToken);
            return followed;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> CreateUser(User user, CancellationToken cancellationToken = default)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<User> GetUserByEmail (string email, CancellationToken cancellationToken = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
