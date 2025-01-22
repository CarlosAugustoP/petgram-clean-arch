using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;
using Domain.Repositorys;

namespace Infrastructure.UserData
{
    public class UserRepository : IUserRepository
    {
        public UserRepository() { }

        public Task<Users> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Users> AddUserToFollowers(Guid followerId, Guid followedId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
