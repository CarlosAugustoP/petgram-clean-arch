using FluentValidation;

namespace Application.Abstractions.Users.AddNewUser
{
    public class AddNewUserCommandValidator : AbstractValidator<AddNewUserCommand>
    {
        public AddNewUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(30)
                .WithMessage("Password must not exceed 30 characters.")
                .Must(password => password.Any(char.IsDigit))
                .WithMessage("Password must contain at least one number.")
                .Must(password => password.Any(char.IsUpper))
                .WithMessage("Password must contain at least one uppercase letter.")
                .Must(password => password.Any(char.IsLower))
                .WithMessage("Password must contain at least one lowercase letter.");

            RuleFor(x => x.Name)
                .MaximumLength(20)
                .WithMessage("Name must be less than 20 characters.");
        }
    }
}
