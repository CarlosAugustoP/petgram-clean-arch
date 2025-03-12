using FluentValidation;

namespace Application.Abstractions.Followers.GetFollowers
{
    public class GetFollowersByUserQueryValidator : AbstractValidator<GetFollowersByUserQuery>
    {
        public GetFollowersByUserQueryValidator(){
            RuleFor(x=> x.PageIndex)
            .Must(x => x>0)
            .WithMessage("Cannot access a negative page")
            .NotEmpty()
            .WithMessage("Please add the page index.");
            RuleFor(x=> x.PageSize)
            .Must(x => x>0)
            .WithMessage("Cannot access negative page values")
            .NotEmpty()
            .WithMessage("Please add the page size.")
            .Must(x => x < 50)
            .WithMessage("Cannot render more than 50 elements at a time");
        }
    }
}