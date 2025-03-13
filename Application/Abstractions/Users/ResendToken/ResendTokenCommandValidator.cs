using FluentValidation;

namespace Application.Abstractions.Users.ResendToken
{
    public class ResendTokenCommandValidator : AbstractValidator<ResendTokenCommand>
    {
        public ResendTokenCommandValidator()
        {
            RuleFor(x => x.UserIdKey).NotEmpty().WithMessage("Please inform the user id key")
            .Must(x=>x!=Guid.Empty).WithMessage("User Id must be a valid GUID");
        }
    }
}