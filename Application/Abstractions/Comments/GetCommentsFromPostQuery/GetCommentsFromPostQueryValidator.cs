using FluentValidation;

namespace Application.Abstractions.Comments.GetCommentsFromPostQuery
{
    public class GetCommentsFromPostQueryValidator : AbstractValidator<GetCommentsFromPostQuery>
    {
        public GetCommentsFromPostQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .Must(x => x > 0)
                .WithMessage("Cannot access a negative page")
                .NotEmpty()
                .WithMessage("Please add the page index.");

            RuleFor(x => x.PageSize)
                .Must(x => x > 0)
                .WithMessage("Cannot access negative page values")
                .NotEmpty()
                .WithMessage("Please add the page size.")
                .Must(x => x < 50)
                .WithMessage("Cannot render more than 50 elements at a time");
            
            RuleFor(x => x.PostId)
            .Must(x=> x != Guid.Empty).WithMessage("Please use a valid GUID");
        }
    }
}