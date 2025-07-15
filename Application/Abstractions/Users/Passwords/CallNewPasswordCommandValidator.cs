using FluentValidation;

namespace Application.Abstractions.Users.Passwords
{
    public class CallNewPasswordCommandValidator : AbstractValidator<CallNewPasswordCommand>
    {
        public CallNewPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }

}