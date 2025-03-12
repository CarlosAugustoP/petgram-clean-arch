using FluentValidation;

namespace Application.Abstractions.Comments.LikeCommentCommand
{
    public class LikeCommentCommandValidator : AbstractValidator<LikeCommentCommand>{
        public LikeCommentCommandValidator()
        {
            RuleFor(x=>x.CommentId).Must(x=> x != Guid.Empty).WithMessage("Must be valid GUID");
            RuleFor(x=>x.UserId).Must(x=> x != Guid.Empty).WithMessage("Must be valid GUID");
            
        }
    }
}