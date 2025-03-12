using FluentValidation;

namespace Application.Abstractions.Comments.DeleteCommentCommand
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>{
        public DeleteCommentCommandValidator(){
            RuleFor(x=>x.CommentId).Must(x => x!= Guid.Empty).WithMessage("Must be a valid GUID");
            RuleFor(x=>x.UserId).Must(x => x!= Guid.Empty).WithMessage("Must be a valid GUID");
        }
    }
}