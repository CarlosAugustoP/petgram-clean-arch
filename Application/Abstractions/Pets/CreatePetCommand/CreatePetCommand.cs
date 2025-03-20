using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public CreatePetCommandHandler(IPetRepository petRepository, ISupabaseService supabaseService, IUserRepository userRepository)
        {
            _petRepository = petRepository;
            _supabaseService = supabaseService;
            _userRepository = userRepository;
        }
        
        internal class PythonResponse {
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
            
            var req = await client.PostAsync("localhost:5001/send", formData);
            var resJson = await req.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<PythonResponse>(resJson);
            
            if (res == null){
                throw new BadRequestException("Error while processing image");
            }

            if (res.@Class == "Cat"){
                req = await client.PostAsync("localhost:5001/get-cat-breed", formData);
                resJson = await req.Content.ReadAsStringAsync(cancellationToken);
                res = JsonSerializer.Deserialize<PythonResponse>(resJson);
                if (res == null){
                    throw new BadRequestException("Error while processing image");
                }
                breed = res.@Class;

            } else if (res.@Class == "Dog"){
                req = await client.PostAsync("localhost:5001/get-dog-breed", formData);
                resJson = await req.Content.ReadAsStringAsync(cancellationToken);
                res = JsonSerializer.Deserialize<PythonResponse>(resJson);
                if (res == null){
                    throw new BadRequestException("Error while processing image");
                }
                breed = res.@Class;
            }

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