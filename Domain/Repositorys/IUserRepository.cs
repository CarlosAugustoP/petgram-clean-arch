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
       Task<Users> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
       Task<Users> AddUserToFollowers(Guid followerId, Guid followedId, CancellationToken cancellationToken = default);
    }
}
