using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helper;
using Domain.Repositorys;
using Domain.CustomExceptions;
using MediatR;

namespace Application.Abstractions.Users.Login
{
    public sealed record LoginCommand : IRequest<object>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, object>
    {
        public IPasswordHasher _passwordHasher;
        public IUserRepository _userRepository;
        public LoginCommandHandler(IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }
        public async Task<object> Handle (LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(command.Email)
                ?? throw new NotFoundException("Invalid login credentials, try again.");
            if (!_passwordHasher.VerifyPassword(command.Password, user.Password))
            {
                throw new NotFoundException("Invalid login credentials, try again.");
            }
            return user.Id.ToString();
        }
    }
}
