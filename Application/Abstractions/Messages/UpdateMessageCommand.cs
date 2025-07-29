using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Messages
{
    public record UpdateMessageCommand(string Content, Guid MessageId, Guid UserId) : IRequest<bool>;
    internal sealed class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, bool>
    {
        private readonly IMessageRepository _messageRepository;

        public UpdateMessageCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<bool> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.MessageId, cancellationToken)
                ?? throw new NotFoundException($"Message with ID {request.MessageId} not found.");
            
            if (message.SenderId != request.UserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this message.");
            }

            message.Content = request.Content;
            await _messageRepository.UpdateAsync(message, cancellationToken);
            return true;
        }
    }
}