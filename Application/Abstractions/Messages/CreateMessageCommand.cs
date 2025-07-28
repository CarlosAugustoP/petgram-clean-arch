using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messages;
using MediatR;

namespace Application.Abstractions.Messages
{
    internal sealed class CreateMessageCommandHandler : IRequestHandler<MessageCommand, MessageDto>
    {
        private readonly MessageService _messageService;

        public CreateMessageCommandHandler(MessageService messageService)
        {
            _messageService = messageService;
        }

        public Task<MessageDto> Handle(MessageCommand request, CancellationToken cancellationToken)
        {
            return _messageService.SendMessageAsync(request, cancellationToken);
        }
    }

}