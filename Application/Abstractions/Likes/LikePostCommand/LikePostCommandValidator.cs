using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Likes.LikePostCommand
{
    public class LikePostCommandValidator : AbstractValidator<LikePostCommand>
    {
        public LikePostCommandValidator()
        {
            RuleFor(x => x.PostId).Must(x => x != Guid.Empty).WithMessage("PostId is required").NotEmpty().WithMessage("PostId is required");
            RuleFor(x => x.UserId).Must(x => x != Guid.Empty).WithMessage("UserId is required").NotEmpty().WithMessage("PostId is required");
        }
    }
}