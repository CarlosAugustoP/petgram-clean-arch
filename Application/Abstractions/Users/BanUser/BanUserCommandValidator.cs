using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Users.BanUser
{
    public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
    {
        public BanUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Reason)
                .IsInEnum().WithMessage("Invalid ban reason.");

            RuleFor(x => x.Remark)
                .MaximumLength(500).WithMessage("Remark cannot exceed 500 characters.");

            RuleFor(x => x.BanExpiration)
                .IsInEnum().WithMessage("Invalid ban expiration value.")
                .When(x => x.BanExpiration.HasValue);
        }
    }

}
