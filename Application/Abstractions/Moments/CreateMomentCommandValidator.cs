using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Moments
{

    public class CreateMomentCommandValidator : AbstractValidator<CreateMomentCommand>
    { 
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;
        public CreateMomentCommandValidator(ProfanityFilter.ProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter;
            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("AuthorId is required.");

            RuleFor(x => x.Media)
                .NotNull().WithMessage("Media file is required.");

            RuleFor(x => x.Content)
                .MaximumLength(500).WithMessage("Content cannot exceed 500 characters.")
                .Must(x => !_profanityFilter.IsProfanity(x)).WithMessage("Content contains inappropriate language.");
        }
    }
}
