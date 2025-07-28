using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messages;
using AutoMapper;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;

namespace Application.Abstractions.Messages
{
    public sealed record MessageHistoryQuery(Guid UserId, Guid ChatPartnerId, int PageIndex, int PageSize) : IRequest<PaginatedList<MessageDto>>;
    internal sealed class MessageHistoryQueryHandler : IRequestHandler<MessageHistoryQuery, PaginatedList<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageHistoryQueryHandler(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<MessageDto>> Handle(MessageHistoryQuery request, CancellationToken cancellationToken)
        {
            var pl = await _messageRepository.GetMessagesByUserChatAsync(request.UserId, request.ChatPartnerId, cancellationToken, request.PageIndex, request.PageSize);
            return pl.Select(_mapper.Map<MessageDto>);
        }
    }
}