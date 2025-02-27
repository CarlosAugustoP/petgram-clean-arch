using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Comments
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;
        public CreateCommentCommandValidator(ProfanityFilter.ProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter ?? throw new ArgumentNullException(nameof(profanityFilter));

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content must not exceed 500 characters.")
                .Must(content => !_profanityFilter.IsProfanity(content))
                .WithMessage("Your comment contains inappropriate language.");

            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("Post ID must be a valid GUID.");
        }
    }
}