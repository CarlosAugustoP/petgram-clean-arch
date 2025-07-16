using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Repositorys;
using Domain.Models;
using Application.Helper;
using Microsoft.EntityFrameworkCore;
using Domain.CustomExceptions;
using Application.Services;
using System.Security.Cryptography;
using Domain.Models.UserAggregate;
using Application.Notifications;

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
        private readonly IEmailService _emailService;
        private readonly IRedisService _redisService;
        public AddNewUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IRedisService redisService, IEmailService emailService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<object> Handle(AddNewUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                Password = _passwordHasher.HashPassword(command.Password),
                Name = command.Name,
                CreatedAt = DateTime.UtcNow,
                // TODO change to default URL for profile image
                ProfileImgUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50"
            };
           
            var existsOnRedis = await _redisService.GetCodeAsync(user.Email);
            
            if (existsOnRedis != null){
                return "A user is already registered and set to receive a token"; 
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new ConflictException("Found credentials for an existing account");
            }

            // generate code
            await _redisService.SetObjectAsync(user.Id.ToString(), user, 10);

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            try
            {
                await _redisService.SetCodeAsync(user.Email, code, 10);
                await _emailService.SendEmail(user.Email,
                    $"A warm welcome from our crew here at pegram! Here is your 6-digit verification token: {code}",
                    "Verification code");
            }
            catch (Exception)
            {
                throw new BadRequestException("Seems like your email is invalid!");
            }
            

            return new Dictionary<string, string> { { "userKey", user.Id.ToString() } };
        }
    }
}
