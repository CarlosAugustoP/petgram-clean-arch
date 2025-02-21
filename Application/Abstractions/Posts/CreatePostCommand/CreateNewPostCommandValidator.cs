using FluentValidation;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public class CreateNewPostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;

        public CreateNewPostCommandValidator(ProfanityFilter.ProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter ?? throw new ArgumentNullException(nameof(profanityFilter));

            RuleFor(x => x.Content)
                .Must(content => !_profanityFilter.IsProfanity(content))
                .WithMessage("Your post contains inappropriate language.");

            RuleFor(x => x.MediaFiles)
                .NotEmpty()
                .WithMessage("Every post needs an image")
                .NotNull()
                .WithMessage("Every post needs an image.");

            RuleFor(x => x.Title)
                .Must(title => !_profanityFilter.IsProfanity(title))
                .WithMessage("Your post contains inappropriate language.")
                .NotNull()
                .WithMessage("Every post needs a title");

        }
    }
}
