using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Comments.LikeCommentCommand
{
    public sealed record LikeCommentCommand : IRequest<Comment>
    {   
        public Guid CommentId { get; init; }
        public Guid UserId { get; init; }
        public LikeCommentCommand(Guid commentId, Guid userId)
        {
            CommentId = commentId;
            UserId = userId;
        }
        
    }

    internal sealed class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, Comment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;

        public LikeCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        public async Task<Comment> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId, cancellationToken)
                ?? throw new NotFoundException($"Comment {request.CommentId} not found");

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");
            
            var existingLike = await _likeRepository.GetLikeByUserAndCommentAsync(request.UserId, request.CommentId, cancellationToken);
            
            if (existingLike == null)
            {
                var like = new Like(Guid.NewGuid(), request.UserId, user, comment, DateTime.UtcNow, request.CommentId);
                await _likeRepository.LikeCommentAsync(like, cancellationToken);
            }else 
            {
                await _likeRepository.DislikeCommentAsync(existingLike, cancellationToken);
            }
            return comment;
        }
    }
}