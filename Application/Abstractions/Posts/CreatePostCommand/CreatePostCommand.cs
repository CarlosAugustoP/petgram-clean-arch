using System.Collections.Concurrent;
using Application.Notifications;
using Application.Notifications.Implementations;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.NotificationAggregate;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using SharedKernel.Utils.Files;
using static Application.Notifications.Implementations.OnSuccessfulUploadPost;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public sealed record CreatePostCommand : IRequest<Post>
    {
        public string Title { get; set; }
        public List<MediaRequest> Medias { get; set; } = new();
        public List<Guid> ReferencedPetIds {get; set;} = new List<Guid>();
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
        private readonly IUserRepository _userRepository;
        private readonly IPetRepository _petRepository;
        private readonly NotificationFactory _notificationFactory;

        public CreatePostCommandHandler(IPostRepository postRepository, ISupabaseService supabaseService,
         IMediaRepository mediaRepository, IPetRepository petRepository, NotificationFactory notificationFactory, IUserRepository userRepository)
        {
            _notificationFactory = notificationFactory;
            _postRepository = postRepository;
            _supabaseService = supabaseService;
            _mediaRepository = mediaRepository;
            _petRepository = petRepository;
            _userRepository = userRepository;
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

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");
                
            //then create the post first
            var post = new Post(
                Guid.NewGuid(), request.UserId, user, request.Title, new List<Media>(),
                request.Content, new List<Comment>(), DateTime.UtcNow, new List<Like>(), 0
            );

            await _postRepository.CreatePostAsync(post, cancellationToken);

            ConcurrentBag<Media> medias = [];
           
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
                    var mediaId = Guid.NewGuid();
                    var nameOfFile = $"{Guid.NewGuid()}_media";
                    var bytes = Convert.FromBase64String(media.Base64String);
                    using var stream = new MemoryStream(bytes);
                    var url = await _supabaseService.UploadFileAsync(stream, nameOfFile, "petgram-posts");
                    
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

            /*
                Notifies all mentioned pets owners and the post creator.
            */
            await _notificationFactory.Create(NotificationTrigger.POST_FINISHED_UPLOAD).ExecuteAsync(
                new OnSuccessfulUploadPostContext(post, petList, user), cancellationToken
            );

            return post;
        }
    }

}
