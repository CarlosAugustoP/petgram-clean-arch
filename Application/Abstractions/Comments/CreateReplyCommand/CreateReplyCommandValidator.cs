using FluentValidation;
using ProfanityFilter.Interfaces;

namespace Application.Abstractions.Comments.CreateReplyCommand
{
    public class CreateReplyCommandValidator : AbstractValidator<CreateReplyCommand>
    {
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;

        public CreateReplyCommandValidator(ProfanityFilter.ProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter;

            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("Comment ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("Comment ID must be a valid GUID.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content must not exceed 500 characters.")
                .Must(content => !_profanityFilter.IsProfanity(content))
                .WithMessage("Your comment contains inappropriate language.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Post ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("Post ID must be a valid GUID.");
        }
    }
}