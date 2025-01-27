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

        public async Task<Users> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<Users> AddUserToFollowers(Users follower, Users followed, CancellationToken cancellationToken)
        {
            followed.Followers.Add(follower);
            follower.Following.Add(followed);
            await _db.SaveChangesAsync(cancellationToken);
            return followed;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<Users> CreateUser(Users user, CancellationToken cancellationToken = default)
        {
            await _db.Users.AddAsync(user);
            return user;
        }
    }
}
