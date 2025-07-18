using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Pets.DeletePetCommand
{
    public sealed record DeletePetCommand(Guid PetId) : IRequest<bool>;
    internal sealed class DeletePetCommandHandler : IRequestHandler<DeletePetCommand, bool>
    {
        private readonly IPetRepository _petRepository;

        public DeletePetCommandHandler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<bool> Handle(DeletePetCommand request, CancellationToken cancellationToken)
        {
            var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken)
                ?? throw new NotFoundException("Pet not found");

            if (pet.OwnerId != request.PetId)
            {
                throw new ForbiddenException("Unauthorized deletion attempt");
            }

            await _petRepository.DeleteAsync(request.PetId, cancellationToken);
            return true;
        }
    }

}