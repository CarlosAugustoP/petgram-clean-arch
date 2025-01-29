using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _db.Users.FindAsync(id);
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
    }
}
