using FluentValidation;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;

        public CreatePostCommandValidator(ProfanityFilter.ProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter ?? throw new ArgumentNullException(nameof(profanityFilter));

            RuleFor(x => x.Content)
                .Must(content => !_profanityFilter.IsProfanity(content))
                .WithMessage("Your post contains inappropriate language.");

            RuleFor(x => x.Medias)
                .NotEmpty()
                .WithMessage("Every post needs an image")
                .NotNull()
                .Must(x => x.Count <= 10 && x.Count > 0)
                .WithMessage("Post can have at most 10 media files and at least one media file")
                .WithMessage("Every post needs an image.");

            RuleFor(x => x.Title)
                .Must(title => !_profanityFilter.IsProfanity(title))
                .WithMessage("Your post contains inappropriate language.")
                .NotNull()
                .WithMessage("Every post needs a title");
                
        }
    }
}
