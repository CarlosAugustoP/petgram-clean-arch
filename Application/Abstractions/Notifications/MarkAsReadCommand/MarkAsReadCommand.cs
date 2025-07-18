using Domain.CustomExceptions;
using Domain.Models.NotificationAggregate;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Notifications
{
    public record MarkAsReadCommand(List<Guid> NotificationIds, Guid UserId) : IRequest<bool>;

    public record MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;
        public MarkAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            List<Notification> notifications = [];
 
            if (request.NotificationIds.Count == 0)
                throw new BadRequestException("No notification IDs provided.");

            foreach (var notificationId in request.NotificationIds)
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken)
                    ?? throw new NotFoundException($"Notification with ID {notificationId} not found.");

                if (notification.IsRead) continue;
                if (notification.ReceiverId != request.UserId) throw new ForbiddenException("You cannot mark this notification as read.");
                notifications.Add(notification);
            }
            await _notificationRepository.MarkManyAsReadAsync(notifications.Select(n => n.Id), cancellationToken);
            return true;
        }
    }
}