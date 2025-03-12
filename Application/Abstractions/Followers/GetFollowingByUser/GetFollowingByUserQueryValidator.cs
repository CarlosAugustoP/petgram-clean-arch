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
                .Must(x => x >= 0).WithMessage("Cannot access a negative page")
                .NotEmpty().WithMessage("Please add the page index.");

            RuleFor(x => x.PageSize)
                .Must(x => x >0).WithMessage("Cannot access negative page values")
                .NotEmpty().WithMessage("Please add the page size.")
                .Must(x => x < 50).WithMessage("Cannot render more than 50 elements at a time");}
    }
}