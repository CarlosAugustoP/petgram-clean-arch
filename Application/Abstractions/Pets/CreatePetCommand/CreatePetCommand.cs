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
        public required string ImgUrl { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Species { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Breed { get; set; }

        [SetsRequiredMembers]
        public CreatePetCommand(string imgUrl, string name, string species, string? description = null, DateTime? birthDate = null)
        {
            ImgUrl = imgUrl;
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
        private readonly IUserRepository _userRepository;
        public CreatePetCommandHandler(IPetRepository petRepository, IUserRepository userRepository)
        {
            _petRepository = petRepository;
            _userRepository = userRepository;
        }
        
        

        public async Task<Pet> Handle(CreatePetCommand request, CancellationToken cancellationToken)
        {
            var newPet = new Pet(
                id: Guid.NewGuid(),
                ownerId: request.UserId,
                owner: await _userRepository.GetByIdAsync(request.UserId, cancellationToken),
                name: request.Name,
                imgUrl: request.ImgUrl ,
                breed: request.Breed ?? "Unknown",  
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