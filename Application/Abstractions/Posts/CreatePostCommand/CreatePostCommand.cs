using System.Collections.Concurrent;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel.Utils.Files;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public sealed record CreatePostCommand : IRequest<Post>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public List<IFormFile> MediaFiles { get; set; }
        public string Content { get; set; }

        public CreatePostCommand(string title, List<IFormFile> mediaFiles, string content, Guid userId)
        {
            Title = title;
            MediaFiles = mediaFiles;
            Content = content;
            UserId = userId;
        }
    }
    internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupabaseService _supabaseService;
        private readonly IMediaRepository _mediaRepository;

        public CreatePostCommandHandler(IPostRepository postRepository, ISupabaseService supabaseService
        , IUserRepository userRepository, IMediaRepository mediaRepository)
        {
            _postRepository = postRepository;
            _supabaseService = supabaseService;
            _userRepository = userRepository;
            _mediaRepository = mediaRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken) ??
                 throw new NotFoundException("Could not find the requested user");

            var postId = Guid.NewGuid();
            
            ConcurrentBag<Media> medias = new();

            var post = new Post(
                postId, request.UserId,
                user, request.Title, new List<Media>(),
                request.Content, new List<Comment>(), DateTime.Now, new List<Like>(), 0
            );

            await Parallel.ForEachAsync(request.MediaFiles, cancellationToken, async (media, token) =>
            {
                string fileType;
                try
                {
                    fileType = FileDiscriminator.DetermineMediaType(media);
                }
                catch (ArgumentException e)
                {
                    throw new BadRequestException(e.Message);
                }
                var url = await _supabaseService.UploadFileAsync(media.OpenReadStream(), media.FileName, fileType);

                var mediaDb = await _mediaRepository.CreateMedia(
                    new Media(Guid.NewGuid(), postId, post, media.FileName, url, fileType, null, DateTime.Now)
                    ,cancellationToken
                );
                medias.Add(mediaDb);
            });
            post.Medias = medias.ToList();
            await _postRepository.CreatePost(post, cancellationToken);
            return post;
        }
    }

}
