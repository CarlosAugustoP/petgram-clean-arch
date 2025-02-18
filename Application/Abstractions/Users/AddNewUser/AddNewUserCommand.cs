using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Repositorys;
using Domain.Models;
using Application.Helper;
using Microsoft.EntityFrameworkCore;
using Domain.CustomExceptions;

namespace Application.Abstractions.Users.AddNewUser
{
    public sealed record AddNewUserCommand : IRequest<object>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string Name { get; init; }

        public AddNewUserCommand(string email, string password, string name)
        {
            Email = email;
            Password = password;
            Name = name;
        }
    }

    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public AddNewUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<object> Handle(AddNewUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = command.Email,
                Password = _passwordHasher.HashPassword(command.Password),
                Name = command.Name,
                // TODO change to default URL for profile image
                ProfileImgUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50"
            };

            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new ConflictException("Found credentials for an existing account");
            }
            
            var result = await _userRepository.CreateUser(user);
            return result;
        }
    }
}
