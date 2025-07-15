using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Users.Passwords
{
    public class AccessLinkCommandValidator : AbstractValidator<AccessLinkCommand>
    {
        public AccessLinkCommandValidator()
        {
            RuleFor(x => x.NewPassword)
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
        }
    }
    
}