using FluentValidation;

namespace Application.Abstractions.Feed
{
    public sealed class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
    {
        public GetFeedQueryValidator()
        {
            RuleFor(query => query.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(query => query.PageIndex)
                .GreaterThanOrEqualTo(0).WithMessage("PageIndex must be greater than or equal to 0.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }
}