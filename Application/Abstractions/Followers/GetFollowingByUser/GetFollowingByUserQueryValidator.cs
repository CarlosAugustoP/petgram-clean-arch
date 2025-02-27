using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Followers.GetFollowingByUser
{
    public class GetFollowingByUserQueryValidator : AbstractValidator<GetFollowingByUserQuery>
    {
        public GetFollowingByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("User ID must be a valid GUID.");

            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}