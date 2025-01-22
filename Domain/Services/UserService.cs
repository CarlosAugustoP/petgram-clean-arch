using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using Domain.CustomExceptions;
namespace Domain.Services
{
    public class UserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Users> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Users? user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
                throw new NotFoundException($"Não foi possível encontrar o usuário de id {id}"); 
            return user;
        }

        public async Task<Users> UserFollowUser(Guid followerId, Guid followedId, CancellationToken cancellationToken)
        {
            Users follower = await GetUserByIdAsync(followerId, cancellationToken);
            Users followed = await GetUserByIdAsync(followedId, cancellationToken);
            return await _userRepository.AddUserToFollowers(
                followerId,
                followedId,
                cancellationToken
            );
        }
    }
}
