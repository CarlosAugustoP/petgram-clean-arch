using Application.Notifications;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Domain.Models.NotificationAggregate;
using static Application.Notifications.Implementations.OnLikedPost;
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
        private readonly NotificationFactory _notificationFactory;

        public LikePostCommandHandler(ILikeRepository likeRepository, IPostRepository postRepository, IUserRepository userRepository, NotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        public async Task<Post> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken) ??
                throw new NotFoundException("User not found for the given id");

            var post = await _postRepository.GetPostByIdAsync(request.PostId, cancellationToken) ??
                throw new NotFoundException("Post not found for the given id");

            var alreadyLiked = await _likeRepository.GetLikeByUserAndPostAsync(request.UserId, request.PostId, cancellationToken);

            if (alreadyLiked == null)
            {
                var like = new Like(
                Guid.NewGuid(), request.UserId, await _userRepository.GetByIdAsync(request.UserId, cancellationToken),
                post, request.PostId, null, DateTime.UtcNow, null
            );
                await _likeRepository.LikePostAsync(like, cancellationToken);

                //send notification for like 
                await _notificationFactory.Create(NotificationTrigger.LIKED_POST).ExecuteAsync(
                    new OnLikedPostContext(post, user, post.Author!), cancellationToken
                );

                return post;
            }
            // if already liked, toggle 
            else if (post.LikesCount > 0)
            {
                await _likeRepository.DislikePostAsync(alreadyLiked, cancellationToken);
               
                return post;
            }
            else
            {
                throw new BadRequestException("Cannot dislike a post with 0 likes.");
            }


        }
    }
}