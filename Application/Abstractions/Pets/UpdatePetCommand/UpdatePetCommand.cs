using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel.Extensions;

namespace Application.Abstractions.Pets.UpdatePetCommand
{
    public sealed record UpdatePetCommand : IRequest<Pet>
    {
        public Guid UserId { get; private set; }
        public Guid PetId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? MainImg { get; set; }
        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }

        public UpdatePetCommand(Guid petId, string? name, string? description, IFormFile? mainImg)
        {
            PetId = petId;
            Name = name;
            Description = description;
            MainImg = mainImg;
        }
    }
    
    internal sealed class UpdatePetCommandHandler : IRequestHandler<UpdatePetCommand, Pet>
    {
        private readonly IPetRepository _petRepository;
        private readonly ISupabaseService _supabaseService;

        public UpdatePetCommandHandler(IPetRepository petRepository, ISupabaseService supabaseService)
        {
            _petRepository = petRepository;
            _supabaseService = supabaseService;
        }

        public async Task<Pet> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
        {
            var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken)
                ?? throw new NotFoundException("Pet not found");

            if (pet.OwnerId != request.UserId)
            {
                throw new ForbiddenException("Unauthorized update attempt");
            }

            if (request.Name != null) pet.Name = request.Name;
            if (request.Description != null) pet.Description = request.Description;


            if (request.MainImg != null)
            {
                pet.ImgUrl = await _supabaseService.UploadFileAsync(request.MainImg.ToStream(),
                     request.MainImg.FileName.ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmss"), "petgram-pets");
            }
            await _petRepository.UpdateAsync(pet, cancellationToken);
            return pet;
        }
    }

}