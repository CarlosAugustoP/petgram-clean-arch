using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Comments.CreateReplyCommand
{
    public sealed record CreateReplyCommand : IRequest<Comment>
    {
        public Guid CommentId { get; init; }
        public string Content { get; init; }
        public Guid UserId { get; private set; }

        public CreateReplyCommand(Guid commentId, string content)
        {
            CommentId = commentId;
            Content = content;
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }

    internal sealed class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, Comment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;


        public CreateReplyCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<Comment> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
        {
            var chosenCommentToReply = await _commentRepository.GetCommentByIdAsync(request.CommentId, cancellationToken) 
                ?? throw new NotFoundException("Chosen comment to reply to was not found");

            var author = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var newComment = new Comment(
                Guid.NewGuid(),
                request.UserId,
                author,
                request.Content,
                DateTime.UtcNow,
                chosenCommentToReply,
                request.CommentId,
                true
            );

            try {
                return await _commentRepository.CreateReplyAsync(newComment, cancellationToken);
            } catch (Exception e) {
                throw new ApiException("Failed to create reply: " + e.Message);
            }
        }
    }
}