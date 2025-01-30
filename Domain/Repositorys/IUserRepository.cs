using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositorys
{
    public interface IUserRepository
    {
       Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
       Task<User> AddUserToFollowers(User follower, User followed, CancellationToken cancellationToken = default);
       Task<User> CreateUser (User user, CancellationToken cancellationToken = default);

       Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default);
    }
}
