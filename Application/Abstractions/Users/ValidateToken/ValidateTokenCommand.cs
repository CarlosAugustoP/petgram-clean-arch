using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;

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

        public ValidateTokenCommandHandler(IRedisService redisService, IUserRepository userRepository)
        {
            _redisService = redisService;
            _userRepository = userRepository;
        }
        public async Task<User> Handle(ValidateTokenCommand request, CancellationToken cancellationToken) 
        {
            var user = await _redisService.GetObjectAsync<User>(request.UserId.ToString())
                ?? throw new NotFoundException("Could not find the requested email for token validation");
            
            if (await _redisService.ValidateAndDeleteCodeAsync(user.Email, request.Token))
                return await _userRepository.CreateUserAsync(user, cancellationToken);
                
            else throw new BadRequestException("Invalid code. please, generate a new code.");
        }
    }
}