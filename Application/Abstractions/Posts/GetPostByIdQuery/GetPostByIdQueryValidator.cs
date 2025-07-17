using FluentValidation;

namespace Application.Abstractions.Posts.GetPostByIdQuery
{
    public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
    {
        public GetPostByIdQueryValidator()
        {
            RuleFor(x => x.Id).Must(x => x != Guid.Empty).WithMessage("PostId is required");
        }
        
    }
    
}