using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Notifications.DTOs;
using AutoMapper;
using Domain.Repositorys;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Application.Abstractions.Notifications
{
    public sealed record GetNotificationsQuery(Guid UserId) : IRequest<List<NotificationDTO>>;
    internal sealed class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDTO>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationDTO>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var list = await _notificationRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            return list.Select(n => _mapper.Map<NotificationDTO>(n)).ToList();
        }
    }
}