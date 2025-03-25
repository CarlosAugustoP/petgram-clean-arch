using FluentValidation;

namespace Application.Abstractions.Pets.GetTypeQuery
{
    public class GetTypeQueryValidator : AbstractValidator<GetTypeQuery>
    {
        public GetTypeQueryValidator()
        {
            RuleFor(x => x.File)
            .NotNull().WithMessage("Please add a file")
            .Must(x => x.Length > 0).WithMessage("Invalid file request");
        }
    }
}