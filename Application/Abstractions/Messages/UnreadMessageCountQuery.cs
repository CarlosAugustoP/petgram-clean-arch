using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Messages
{
    public class UnreadMessageCountQuery(Guid uId) : IRequest<int>
    {
        public Guid UserId { get; } = uId;
    }

    public class UnreadMessageCountQueryHandler : IRequestHandler<UnreadMessageCountQuery, int>
    {
        private readonly IMessageRepository _messageRepository;

        public UnreadMessageCountQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<int> Handle(UnreadMessageCountQuery request, CancellationToken cancellationToken)
        {
            return _messageRepository.GetUnreadMessageCountAsync(request.UserId, cancellationToken);
        }
    }
}