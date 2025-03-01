using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Comments.UpdateCommentCommand
{
    public sealed record UpdateCommentCommand : IRequest<Comment>
    {
        public Guid CommentId { get; init; }
        public string Content { get; init; }
        public Guid UserId { get; private set; }

        public UpdateCommentCommand(Guid commentId, string content)
        {
            CommentId = commentId;
            Content = content;
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }

    internal sealed class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Comment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public UpdateCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<Comment> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId, cancellationToken)
                ?? throw new NotFoundException("Comment not found");

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            if (user.Id != comment.AuthorId)
            {
                throw new BadRequestException("Cannot update other user's comment");
            }

            return await _commentRepository.UpdateCommentAsync(comment, request.Content, cancellationToken);
        }
    }
}