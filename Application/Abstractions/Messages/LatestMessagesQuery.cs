using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messages;
using AutoMapper;
using Domain.CustomExceptions;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;

namespace Application.Abstractions.Messages
{
    public record LatestMessagesQuery(Guid UserId, int PageIndex, int PageSize) : IRequest<PaginatedList<MessageHeaderDto>>;
    internal sealed class LatestMessagesQueryHandler : IRequestHandler<LatestMessagesQuery, PaginatedList<MessageHeaderDto>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public LatestMessagesQueryHandler(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<MessageHeaderDto>> Handle(LatestMessagesQuery request, CancellationToken cancellationToken)
        {
            var pl = await _messageRepository.GetLatestMessagesAsync(request.UserId, cancellationToken, request.PageIndex, request.PageSize)
                ?? throw new NotFoundException("No messages found for the user.");
            return pl.Select(x => new MessageHeaderDto(x.Item1.Receiver.Name, x.Item1.Receiver.ProfileImgUrl, x.Item1.CreatedAt, x.Item1.Content, x.Item1.Receiver.Id, x.Item2));
        }
    }   
}