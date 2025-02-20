using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel.Utils.Files;
using Supabase.Storage;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public sealed record CreatePostCommand : IRequest<Post>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public List<IFormFile> MediaFiles { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
        public int Shares { get; set; }

        public CreatePostCommand(string title, List<IFormFile> mediaFiles, string content,
            DateTime createdAt, List<Like> likes, int shares, Guid userId)
        {
            Title = title;
            MediaFiles = mediaFiles;
            Content = content;
            CreatedAt = createdAt;
            Likes = likes;
            Shares = shares;
            UserId = userId;
        }
    }
    internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupabaseService _supabaseService;

        public CreatePostCommandHandler(IPostRepository postRepository, ISupabaseService supabaseService
        , IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _supabaseService = supabaseService;
            _userRepository = userRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            // TODO firebase service, indepent media for post creation,
            // i can send a list of medias, see if they are imsges or videos,
            // send to firebase get the url and save the post with the urls,
            // then persist them in their own table and connect to the post.
            // Also add a constructor for media 

            var user = await _userRepository.GetByIdAsync(request.UserId) ??
                throw new NotFoundException("Could not find the requested user");

            var postId = Guid.NewGuid();
            var post = new Post(
                postId,
                request.UserId,
                await _userRepository.GetByIdAsync(request.UserId),
                request.Title,
                new List<Media>(),
                request.Content,
                new List<Comment>(),
                request.CreatedAt,
                request.Likes,
                request.Shares
            );
            foreach (var media in request.MediaFiles)
            {
                string fileType;

                if (FileDiscriminator.IsImage(media))
                {
                    fileType = "image";
                }
                else if (FileDiscriminator.IsVideo(media))
                {
                    fileType = "video";
                }
                else
                {
                    throw new BadRequestException("File type not supported");
                }

                var url = _supabaseService.UploadFileAsync(media.OpenReadStream(), media.FileName, fileType).Result;
                var mediaDb = new Media(Guid.NewGuid(), postId, post, media.FileName, url, fileType, null, DateTime.Now);

                // media repository create media here
                post.Medias.Add(mediaDb);

            }
            await _postRepository.CreatePost(post);

            return post;
        }
    }

}
