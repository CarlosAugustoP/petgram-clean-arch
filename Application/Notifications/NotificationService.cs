using System;
using System.Linq;
using Application.Notifications.Request;
using Domain.CustomExceptions;
using Domain.Repositorys;
using Domain.Models.NotificationAggregate;
using Application.Notifications.WebSockets;
using AutoMapper;
using Application.Notifications.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications
{
    public class NotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IMapper _mapper;

        //ctor 
        public NotificationService(IUserRepository userRepository, INotificationRepository notificationRepository, IHubContext<NotificationHub> notificationHub, IMapper mapper)
        {
            _mapper = mapper;
            _notificationHub = notificationHub;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }
        public async Task CreateOne(CancellationToken cancellationToken, NotificationRequest request, NotificationTrigger type)
        {
            var user = await _userRepository.GetByIdAsync(request.To, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var notification = new Notification(
                Guid.NewGuid(),
                DateTime.UtcNow,
                request.Message,
                type,
                user.Id,
                user
            );


            await _notificationRepository.AddAsync(notification);

        }

        public async Task CreateMany(CancellationToken cancellationToken, IEnumerable<Notification> requests, NotificationTrigger type)
        {
            var notifications = requests.Select(async request =>
            {
                var user = await _userRepository.GetByIdAsync(request.ReceiverId, cancellationToken);
                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }

                return new Notification(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    request.Message,
                    type,
                    user.Id,
                    user
                );
            });

            foreach (var notificationTask in notifications)
            {
                var notification = await notificationTask;
                await _notificationRepository.AddAsync(notification);
                await _notificationHub.Clients.User(notification.ReceiverId.ToString())
            .SendAsync("ReceiveNotification", notification);
            }            
        }
        
    }
}