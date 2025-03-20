using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedKernel.Utils.Http;

namespace Application.Abstractions.Pets.CreatePetCommand
{
    public sealed record CreatePetCommand : IRequest<Pet>
    {
        public Guid UserId { get; private set; }
        public required IFormFile Img { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Species { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Breed { get; set; }

        [SetsRequiredMembers]
        public CreatePetCommand(IFormFile img, string name, string species, string? description = null, DateTime? birthDate = null)
        {
            Img = img;
            Name = name;
            Species = species;
            Description = description;
            BirthDate = birthDate;
        }

        public void SetUserId(Guid uid)
        {
            UserId = uid;
        }
    }

    internal sealed class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, Pet>
    {
        private readonly IPetRepository _petRepository;
        private readonly ISupabaseService _supabaseService;
        private readonly IUserRepository _userRepository;
        private readonly string ApiUrl;
        public CreatePetCommandHandler(IPetRepository petRepository, ISupabaseService supabaseService, IUserRepository userRepository, string apiUrl)
        {
            _petRepository = petRepository;
            _supabaseService = supabaseService;
            _userRepository = userRepository;
            ApiUrl = apiUrl;
        }
        
        internal class ModelResponse {
            public required string @Class { get; set; }
            public required decimal Confidence { get; set; }
            public required string Message { get; set; }

        }

        public async Task<Pet> Handle(CreatePetCommand request, CancellationToken cancellationToken)
        {
            string breed = null!;

            var client = new HttpClient();
            
            var formData = new MultipartFormDataContent
            {
                { new StringContent(request.Img.FileName), "file" }
            };

            try 
            { 
                var classificationResponse = HttpHelper.HttpPostAsync<ModelResponse>(ApiUrl, formData);
            }
            catch(Exception e)
            {
                throw new BadRequestException(e.Message);
            };

            var pId = Guid.NewGuid();
            var stream = new MemoryStream();
            await request.Img.CopyToAsync(stream, cancellationToken);

            var imgUrl = await _supabaseService.UploadFileAsync(stream, $"{pId}_{request.Name}", "petgram-pets"); ;

            var newPet = new Pet(
                id: pId,
                ownerId: request.UserId,
                owner: await _userRepository.GetByIdAsync(request.UserId, cancellationToken),
                name: request.Name,
                imgUrl: imgUrl,
                breed: breed ?? request.Breed ?? "Unknown",  
                species: request.Species,
                description: request.Description,
                createdAt: DateTime.UtcNow,
                birthDate: request.BirthDate,
                cuteMeter: 0,
                posts: null
            );

            return await _petRepository.CreateAsync(newPet, cancellationToken);
        }
    }
}