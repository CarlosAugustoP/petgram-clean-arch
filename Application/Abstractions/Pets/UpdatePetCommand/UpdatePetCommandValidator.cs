using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Pets.UpdatePetCommand
{
    public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
    {
        public UpdatePetCommandValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must be less than 500 characters.");
            RuleFor(x => x.Name)
                .MaximumLength(70)
                .WithMessage("Name must be less than 70 characters.");
        }
    }
}