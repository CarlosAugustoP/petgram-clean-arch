using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel.Extensions;

namespace Application.Abstractions.Moments
{
    public sealed class CreateMomentCommand : IRequest<Moment>
    {
        public Guid AuthorId { get; private set; }
        public void SetAuthorId(Guid authorId)
        {
            AuthorId = authorId;
        }
        public IFormFile Media { get; set; }
        public string Content { get; set; }

        public CreateMomentCommand(Guid authorId, IFormFile media, string content)
        {
            AuthorId = authorId;
            Media = media;
            Content = content;
        }
      
    }

    internal sealed class CreateMomentCommandHandler : IRequestHandler<CreateMomentCommand, Moment>
    {
        private readonly IMomentRepository _momentRepository;
        private readonly ISupabaseService _supabaseService;
        private readonly IUserRepository _userRepository;

        public CreateMomentCommandHandler(IMomentRepository momentRepository, ISupabaseService supabaseService, IUserRepository userRepository)
        {

            _momentRepository = momentRepository;
            _supabaseService = supabaseService;
            _userRepository = userRepository;
        }

        public async Task<Moment> Handle(CreateMomentCommand request, CancellationToken cancellationToken)
        {
            var file = request.Media.ToStream();
            var id = Guid.NewGuid();
            var url = await _supabaseService.UploadFileAsync(file, $"moment_{id}", "petgram-moments");

            var moment = new Moment
            {
                Id = id,
                AuthorId = request.AuthorId,
                Media = new Media
                {
                    Url = url,
                    Type = request.Media.ContentType
                },
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                Shares = 0,
                IsVisible = true,
                Author = await _userRepository.GetByIdAsync(request.AuthorId, cancellationToken) ?? throw new NotFoundException("User not found!")
            };

            return await _momentRepository.CreateAsync(moment, cancellationToken);
        }
    }
}