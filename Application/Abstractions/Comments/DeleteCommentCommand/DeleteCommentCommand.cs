using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Comments.DeleteCommentCommand
{
    public sealed record DeleteCommentCommand : IRequest<Comment>
    {
        public Guid CommentId { get; init; }
        public Guid UserId { get; set; }

        public DeleteCommentCommand(Guid commentId, Guid userId)
        {
            CommentId = commentId;
            UserId = userId;
        }

    }

    internal sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Comment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public DeleteCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<Comment> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId, cancellationToken)
                ?? throw new NotFoundException("Comment not found");

            if (user.Id != comment.AuthorId)
            {
                throw new BadRequestException("Cannot delete other user's comment");
            }
            try
            {
                return await _commentRepository.DeleteCommentAsync(comment, cancellationToken);
            }
            catch (Exception e)
            {
                throw new ApiException("Cannot delete comment: " + e);
            }
        }
    }
}