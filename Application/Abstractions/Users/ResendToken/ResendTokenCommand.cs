using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.UserAggregate;
using MediatR;

namespace Application.Abstractions.Users.ResendToken 
{
    public sealed record ResendTokenCommand : IRequest<Dictionary<string,string>>
    {
        public required Guid UserIdKey {get; init;}

        [SetsRequiredMembers]
        public ResendTokenCommand(Guid userId)
        {
            UserIdKey = userId;
        }
    
    }

    internal sealed class ResendTokenCommandHandler : IRequestHandler<ResendTokenCommand, Dictionary<string, string>>
    {
        private readonly IRedisService _redisService;
        private readonly IEmailService _emailService;

        public ResendTokenCommandHandler(IRedisService redisService, IEmailService emailService)
        {
            _redisService = redisService;
            _emailService = emailService;
        }

        public async Task<Dictionary<string, string>> Handle(ResendTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _redisService.GetObjectAsync<User>(request.UserIdKey.ToString());
            if (user == null)
                throw new BadRequestException("User no longer exists in redis");
            
            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            
            await _redisService.SetCodeAsync(user.Email, code, 10);
            await _redisService.SetObjectAsync(user.Id.ToString(), user, 10);
            await _emailService.SendEmail(user.Email,
                $"Here is your new 6-digit verification token: {code}",
                "Verification code");
            
            return new Dictionary<string, string>{
                {user.Id.ToString(), user.Email}
            };
        }
    }
}