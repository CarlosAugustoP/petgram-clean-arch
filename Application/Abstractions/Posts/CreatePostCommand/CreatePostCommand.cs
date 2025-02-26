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
        public string Title { get; set; }
        public List<IFormFile> MediaFiles { get; set; } = new();
        public string Content { get; set; }
        public Guid UserId { get; private set; }

        public CreatePostCommand() { }

        public CreatePostCommand(string title, List<IFormFile> mediaFiles, string content)
        {
            Title = title;
            MediaFiles = mediaFiles;
            Content = content;
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }


    internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;
        private readonly ISupabaseService _supabaseService;
        private readonly IMediaRepository _mediaRepository;

        public CreatePostCommandHandler(IPostRepository postRepository, ISupabaseService supabaseService,
         IMediaRepository mediaRepository)
        {
            _postRepository = postRepository;
            _supabaseService = supabaseService;
            _mediaRepository = mediaRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
             var post = new Post(
                Guid.NewGuid(), request.UserId, null , request.Title, new List<Media>(),
                request.Content, new List<Comment>(), DateTime.UtcNow, new List<Like>(), 0
            );

            await _postRepository.CreatePost(post, cancellationToken);

            ConcurrentBag<Media> medias = new();
           
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
                try 
                {
                    var url = await _supabaseService.UploadFileAsync(media.OpenReadStream(), media.FileName, "petgram-posts");
                    var mediaDb = await _mediaRepository.CreateMedia(
                    new Media(Guid.NewGuid(), post.Id, null!, media.FileName, url, fileType, null, DateTime.UtcNow)
                    , cancellationToken);
                    medias.Add(mediaDb);
                }
                catch(Exception e)
                {
                    throw new ApiException(e.Message);
                };
               
               
            });
            post.Medias = medias.ToList();
            await _postRepository.UpdatePost(post, cancellationToken);
            return post;
        }
    }

}
