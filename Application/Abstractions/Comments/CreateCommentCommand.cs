using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Comments
{
    public sealed record CreateCommentCommand : IRequest<Comment>
    {
        public Guid UserId { get; private set; }
        public Guid PostId { get; init; }
        public string Content { get; init; }
        public CreateCommentCommand(Guid postId, string content)
        {
            PostId = postId;
            Content = content;
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
        
    }

    internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Comment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
        }
        public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            _ = await _postRepository.GetPostByIdAsync(request.PostId, cancellationToken) ??
                throw new NotFoundException("Post not found for the given id");

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken) ??
                throw new NotFoundException("User not found for the given id");
                
            var comment = new Comment(
                Guid.NewGuid(), 
                request.UserId, 
                user, 
                request.Content, 
                DateTime.UtcNow, 
                new List<Comment>(), 
                new List<Like>()
            );
            var createdComment = await _commentRepository.CreateCommentAsync(comment, cancellationToken);
            return createdComment;
        }
    }
}