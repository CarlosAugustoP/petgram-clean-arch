using Application.Services;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions {
    public sealed record ValidateTokenCommand : IRequest<User>
    {
        public required string Token {get; set;}

        public ValidateTokenCommand(Guid userId, string token)
        {
            Token = token;
        }
    }
    internal sealed class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, User>
    {

        private readonly IRedisService _redisService;
        private readonly IUserRepository _userRepository;

        public ValidateTokenCommandHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task<User> Handle(ValidateTokenCommand request, CancellationToken cancellationToken) 
        {
            var user = await _redisService.GetObjectAsync<User>(request.Token);
            return new User();
        }
    }
}