using Application.Helper;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Users.Passwords
{
    public sealed record AccessLinkCommand(string NewPassword, string Token, string UserId) : IRequest<bool>;

    internal sealed class AccessLinkCommandHandler : IRequestHandler<AccessLinkCommand, bool>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRedisService _redisService;
        private readonly IUserRepository _userRepository;

        public AccessLinkCommandHandler(IPasswordHasher passwordHasher, IRedisService redisService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _redisService = redisService;
        }

        public async Task<bool> Handle(AccessLinkCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken)
                ?? throw new NotFoundException("User not found for the given id");

            var redisKey = $"password-reset:{request.UserId}";
            var token = await _redisService.GetObjectAsync<string>(redisKey);

            if (token == null)
            {
                throw new NotFoundException("Invalid or expired password reset link.");
            }
            if (token != request.Token)
            {
                throw new UnauthorizedAccessException("Invalid token provided.");
            }
            user.Password = _passwordHasher.HashPassword(request.NewPassword);
            await _userRepository.UpdateUserAsync(user, cancellationToken);
            await _redisService.DeleteAsync(redisKey);
            return true;
        }
    }
}