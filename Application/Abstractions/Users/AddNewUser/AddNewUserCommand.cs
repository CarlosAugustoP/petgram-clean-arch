using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Repositorys;
using Domain.Models;

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

        public AddNewUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<object> Handle(AddNewUserCommand command, CancellationToken cancellationToken)
        {
            var user = new Domain.Models.Users
            {
                Email = command.Email,
                Password = command.Password,
                Name = command.Name
            };

            var result = await _userRepository.CreateUser(user);
            return result;
        }
    }
}
