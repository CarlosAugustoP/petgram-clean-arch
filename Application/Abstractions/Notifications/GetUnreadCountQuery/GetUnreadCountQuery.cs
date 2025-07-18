using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Notifications.GetUnreadCountQuery
{
    public sealed record GetUnreadCountQuery(Guid UserId) : IRequest<int>;

    public record GetUnreadCountQueryHandler : IRequestHandler<GetUnreadCountQuery, int>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUnreadCountQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetUnreadCountAsync(request.UserId, cancellationToken);
        }
    }
}