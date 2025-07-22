using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Users.GetAllUsersQuery
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(x => x)
                .NotNull();

            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.SearchQuery)
                .NotNull()
                .MaximumLength(100).WithMessage("SearchQuery cannot exceed 100 characters.");
        }
    }
}