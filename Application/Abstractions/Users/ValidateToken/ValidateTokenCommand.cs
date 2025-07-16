using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Application.Notifications;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.NotificationAggregate;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;
using static Application.Notifications.Implementations.OnNewUser;

namespace Application.Abstractions.Users.ValidateToken {
    public sealed record ValidateTokenCommand : IRequest<User>
    {
        public required string Token {get; init;}
        public required Guid UserId {get; init;}

        [SetsRequiredMembers]
        public ValidateTokenCommand( string token, Guid userId)
        {
            Token = token;
            UserId = userId;
        }
    }
    internal sealed class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, User>
    {

        private readonly IRedisService _redisService;
        private readonly IUserRepository _userRepository;
        private readonly NotificationFactory _notificationFactory;

        public ValidateTokenCommandHandler(IRedisService redisService, IUserRepository userRepository, NotificationFactory notificationFactory)
        {
            _redisService = redisService;
            _userRepository = userRepository;
            _notificationFactory = notificationFactory;
        }
        public async Task<User> Handle(ValidateTokenCommand request, CancellationToken cancellationToken) 
        {
            var user = await _redisService.GetObjectAsync<User>(request.UserId.ToString())
                ?? throw new NotFoundException("Could not find the requested email for token validation");

            if (await _redisService.ValidateAndDeleteCodeAsync(user.Email, request.Token))
            {
                var result = await _userRepository.CreateUserAsync(user, cancellationToken);
                await _notificationFactory.Create(NotificationTrigger.NEW_USER).ExecuteAsync(new OnNewUserContext(user.Name, user.Id), cancellationToken);
                return result;
            }
            else throw new BadRequestException("Invalid code. please, generate a new code.");
        }
    }
}