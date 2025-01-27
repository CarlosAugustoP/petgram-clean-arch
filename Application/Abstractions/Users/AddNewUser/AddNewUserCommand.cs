using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Domain.Models;
using Domain.Repositorys;

namespace Application.Abstractions.Users.AddNewUser
{
    public sealed record AddNewUserCommand : Messaging.ICommand
    {
        public AddNewUserCommand() { }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
    public class AddNewUserCommandHandler : ICommandHandler<AddNewUserCommand> 
    {
        private readonly IUserRepository _userRepository;
        public AddNewUserCommandHandler(IUserRepository userRepository) {
            _userRepository = userRepository;
        }
        public async Task<object> Handle(AddNewUserCommand command, CancellationToken cancellationToken)
        {
            Domain.Models.Users user = new Domain.Models.Users
            {
                Email = command.Email,
                Password = command.Password,
                Name = command.Name,
            };
            return await _userRepository.CreateUser(user);
        }
    
    }
}
