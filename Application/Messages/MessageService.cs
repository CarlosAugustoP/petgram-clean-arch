using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositorys;
using Microsoft.AspNetCore.SignalR;
using Domain.Models;
using Infrastructure.NotificationData;
using Application.Notifications;
using Domain.CustomExceptions;
using Domain.Models.NotificationAggregate;
using static Application.Notifications.Implementations.OnNewMessage;
using AutoMapper;
namespace Application.Messages
{
    public class MessageService
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly NotificationFactory _notificationFactory;
        private readonly IMapper _mapper;

        public MessageService(IHubContext<MessageHub> hubContext, IMessageRepository messageRepository, IUserRepository userRepository, NotificationFactory notificationFactory, IMapper mapper)
        {
            _hubContext = hubContext;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _notificationFactory = notificationFactory;
            _mapper = mapper;
        }

        public async Task<MessageDto> SendMessageAsync(MessageCommand request, CancellationToken cancellationToken)
        {
            var receiver = await _userRepository.GetByIdAsync(request.ReceiverId, cancellationToken)
                ?? throw new NotFoundException($"Receiver with ID {request.ReceiverId} not found.");

            var sender = await _userRepository.GetByIdAsync(request.SenderId, cancellationToken)
                ?? throw new NotFoundException($"Sender with ID {request.SenderId} not found.");

            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                Sender = sender,
                Receiver = receiver,
                IsRead = false,
                IsEdited = false
            };

            await _messageRepository.CreateAsync(message, cancellationToken);

            await _hubContext.Clients.User(receiver.Id.ToString())
                .SendAsync("ReceiveMessage", message, cancellationToken);

            await _notificationFactory.Create(NotificationTrigger.NEW_MESSAGE)
                .ExecuteAsync(new OnNewMessageContext(message), cancellationToken);

            return _mapper.Map<MessageDto>(message);
        }
    }
}