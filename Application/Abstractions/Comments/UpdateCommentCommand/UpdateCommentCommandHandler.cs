using System.Data;
using FluentValidation;
using ProfanityFilter.Interfaces;

namespace Application.Abstractions.Comments.UpdateCommentCommand
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;

        public UpdateCommentCommandValidator(ProfanityFilter.ProfanityFilter profanityFilter){
            
            _profanityFilter = profanityFilter;

            RuleFor(x=>x.Content)
            .Must(x => !_profanityFilter.IsProfanity(x))
            .WithMessage("Your comment contains inappropriate language")
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(500)
            .WithMessage("Content must not exceed 500 characters.");

            RuleFor(x=>x.CommentId).Must(x=> x != Guid.Empty).WithMessage("Must be valid GUID");
            RuleFor(x=>x.UserId).Must(x=> x != Guid.Empty).WithMessage("Must be valid GUID");

    
        }
    }
}