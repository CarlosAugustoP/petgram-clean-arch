using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Likes.LikePostCommand
{
    public sealed record LikePostCommand : IRequest<Post>
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }

    internal sealed class LikePostCommandHandler : IRequestHandler<LikePostCommand, Post>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public LikePostCommandHandler(ILikeRepository likeRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        public async Task<Post> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostById(request.PostId, cancellationToken) ??
                throw new NotFoundException("Post not found for the given id");

            var alreadyLiked = await _likeRepository.GetLikeByUserAndPost(request.UserId, request.PostId, cancellationToken);

            if (alreadyLiked == null)
            {
                var like = new Like(
                Guid.NewGuid(), request.UserId, await _userRepository.GetByIdAsync(request.UserId, cancellationToken),
                post, request.PostId, null, DateTime.UtcNow, null
            );
                await _likeRepository.LikePost(like, cancellationToken);
                return post;
            }
            else if (post.LikesCount > 0)
            {
                await _likeRepository.DislikePost(alreadyLiked, cancellationToken);            
                return post;
            }
            else
            {
                throw new BadRequestException("Cannot dislike a post with 0 likes.");
            }
        }
    }
}