using Domain.Models;
using FluentValidation;

namespace Application.Abstractions.Pets
{
    public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand.CreatePetCommand>{
        public CreatePetCommandValidator()
        {
            RuleFor(x => x.BirthDate)
                .Must(date => date != default(DateTime))
                .WithMessage("BirthDate must be a valid date.");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must be less than 500 characters.");
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Name is required.");
            RuleFor(x => x.Species)
                .NotNull()
                .WithMessage("Species is required.");
            RuleFor(x => x.Img)
                .NotNull()
                .WithMessage("Image is required.");
            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("UserId is required.")
                .Must(userId => userId != Guid.Empty)
                .WithMessage("UserId must be a valid Guid.");
            
               
        }
    }
}