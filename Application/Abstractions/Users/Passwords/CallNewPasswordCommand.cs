using System.Security.Cryptography;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Abstractions.Users.Passwords
{
    public sealed record CallNewPasswordCommand(string Email) : IRequest<bool>;

    internal sealed class CallNewPasswordCommandHandler : IRequestHandler<CallNewPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IRedisService _redisService;
        private readonly ILogger<CallNewPasswordCommandHandler> _logger;
        public CallNewPasswordCommandHandler(IEmailService emailService, IRedisService redisService, IUserRepository userRepository, ILogger<CallNewPasswordCommandHandler> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
            _emailService = emailService;
            _redisService = redisService;
        }
        public async Task<bool> Handle(CallNewPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken)
                    ?? throw new NotFoundException("User not found for the given id");

                var tokenBytes = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(tokenBytes);
                }
                var token = Convert.ToBase64String(tokenBytes);

                var redisKey = $"password-reset:{user.Id}";
                await _redisService.SetObjectAsync(redisKey, token, 15);

                await _emailService.SendEmail(user.Email, "Password Reset Request",
                    $"To reset your password, please access the following link: " +
                    $"https://localhost:3000/reset-password?token={token}&userId={user.Id}");

                return true;
            }
            catch
            {
                _logger.LogError("Failed to send password reset link for user {Email}", request.Email);
                //Safety reasons
                return true;
            } 
        }
    }

}