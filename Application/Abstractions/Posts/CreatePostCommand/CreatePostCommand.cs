using System.Collections.Concurrent;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using SharedKernel.Utils.Files;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public sealed record CreatePostCommand : IRequest<Post>
    {
        public string Title { get; set; }
        public List<MediaRequest> Medias { get; set; } = new();
        public List<Guid> ReferencedPetIds {get; set;}
        public string Content { get; set; }
        public Guid UserId { get; private set; }

        public CreatePostCommand() { }

        public CreatePostCommand(string title, List<MediaRequest> medias, string content)
        {
            Title = title;
            Medias = medias;
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
        private readonly IPetRepository _petRepository;

        public CreatePostCommandHandler(IPostRepository postRepository, ISupabaseService supabaseService,
         IMediaRepository mediaRepository, IPetRepository petRepository)
        {
            _postRepository = postRepository;
            _supabaseService = supabaseService;
            _mediaRepository = mediaRepository;
            _petRepository = petRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            //First step, validate and get all the pets for the new media.
            var petList = new List<Pet>();  
            request.Medias.ForEach(media =>
            {
                media.ReferencedPets.ForEach(async petId =>
                {
                    var pet = await _petRepository.GetByIdAsync(petId, cancellationToken) 
                        ?? throw new NotFoundException("Pet not found");
                    petList.Add(pet);
                });
            });

            //then create the post first
             var post = new Post(
                Guid.NewGuid(), request.UserId, null , request.Title, new List<Media>(),
                request.Content, new List<Comment>(), DateTime.UtcNow, new List<Like>(), 0
            );

            await _postRepository.CreatePostAsync(post, cancellationToken);

            ConcurrentBag<Media> medias = new();
           
           //send all the medias attribute to the post
            await Parallel.ForEachAsync(request.Medias, cancellationToken, async (media, token) =>
            {
                string fileType;
                try
                {
                    fileType = FileDiscriminator.DetermineMediaType(
                        Convert.FromBase64String(media.Base64String)
                    );
                }
                catch (ArgumentException e)
                {
                    throw new BadRequestException(e.Message);
                }
                try 
                {
                    //send the file to the cloud
                    var mediaId = Guid.NewGuid();
                    var nameOfFile = $"{Guid.NewGuid()}_media";
                    var bytes = Convert.FromBase64String(media.Base64String);
                    using var stream = new MemoryStream(bytes);
                    var url = await _supabaseService.UploadFileAsync(stream, nameOfFile, "petgram-posts");
                    
                    //for each media we add the array of pets
                    var mediaDb = await _mediaRepository.CreateMediaAsync(
                        new Media(mediaId, post.Id, null!, nameOfFile,
                            url, fileType, null, DateTime.UtcNow, petList)
                        ,cancellationToken);
                    
                    medias.Add(mediaDb);
                }
                catch(Exception e)
                {
                    throw new ApiException(e.Message);
                };
               
               
            });
            post.Medias = medias.ToList();
            await _postRepository.UpdatePostAsync(post, cancellationToken);
            return post;
        }
    }

}
