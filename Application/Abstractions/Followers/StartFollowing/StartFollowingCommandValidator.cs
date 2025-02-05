using System;
using FluentValidation;

namespace Application.Abstractions.Followers.StartFollowing
{
    public class StartFollowingCommandValidator : AbstractValidator<StartFollowingCommand>
    {
        public StartFollowingCommandValidator()
        {
            RuleFor(x => x.FollowerId)
                .NotEmpty()
                .WithMessage("Follower Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Follower Id must be a valid GUID");

            RuleFor(x => x.FollowedId)
                .NotEmpty()
                .WithMessage("Followed Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Followed Id must be a valid GUID");
        }
    }
}
