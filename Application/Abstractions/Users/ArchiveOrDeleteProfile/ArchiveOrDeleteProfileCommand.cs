using System.Security.Cryptography;
using Application.Helper;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Users.ArchiveOrDeleteProfile
{
    public sealed record ArchiveOrDeleteProfileSolicitationCommand(Guid UserId, UserStatus Status, string Password) : IRequest<bool>;
    internal sealed class ArchiveOrDeleteProfileSolicitationCommandHandler : IRequestHandler<ArchiveOrDeleteProfileSolicitationCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRedisService _redisService;

        public ArchiveOrDeleteProfileSolicitationCommandHandler(IUserRepository userRepository, IEmailService emailService, IPasswordHasher passwordHasher, IRedisService redisService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _redisService = redisService;
        }

        public async Task<bool> Handle(ArchiveOrDeleteProfileSolicitationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            if (!_passwordHasher.VerifyPassword(user.Password, request.Password))
                throw new ForbiddenException("Invalid password");

                var tokenBytes = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(tokenBytes);
                }
                var token = Convert.ToBase64String(tokenBytes);
                var redisKey = $"profile-action:{user.Id}";
                await _redisService.SetObjectAsync(redisKey, new TokenData(request.Status, token), 15);
                var subject = request.Status == UserStatus.ARCHIVED ? "Archive Profile Request" : "Delete Profile Request";
                var body = $"To confirm your request, use this token: {token}";
                await _emailService.SendEmail(user.Email, body, subject);

                return true;
        }
    }

        
}