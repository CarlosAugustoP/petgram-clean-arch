using FluentValidation;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace  Application.Abstractions.Users.ValidateToken
{
    public class ValidateTokenCommandValidator : AbstractValidator<ValidateTokenCommand>
    {
        public ValidateTokenCommandValidator()
        {
            RuleFor(x => x.Token)
                .Length(6)
                .NotEmpty()
                .WithMessage("Please inform the token");
        }
    }
}