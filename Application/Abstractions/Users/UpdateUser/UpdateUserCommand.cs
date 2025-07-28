using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Users.UpdateUser
{
    public sealed class UpdateUserCommand : IRequest<bool>
    {
        public Guid UserId { get; private set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImgUrl { get; set; }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }

        public UpdateUserCommand(Guid userId, string? name, string? bio, string? profileImgUrl)
        {
            UserId = userId;
            Name = name;
            Bio = bio;
            ProfileImgUrl = profileImgUrl;
        }
    }

    internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {request.UserId} not found.");
            }

            user.Name = request.Name ?? user.Name;
            user.Bio = request.Bio ?? user.Bio;
            user.ProfileImgUrl = request.ProfileImgUrl ?? user.ProfileImgUrl;

            await _userRepository.UpdateUserAsync(user, cancellationToken);
            return true; 
        }
    }

}