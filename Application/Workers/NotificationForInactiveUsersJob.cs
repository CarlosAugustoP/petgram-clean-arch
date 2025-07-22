using Application.Notifications;
using Domain.Models.NotificationAggregate;
using Domain.Repositorys;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Workers
{
    public class NotificationForInactiveUsersJob
    {
        private readonly IRecurringJobManager _backgroundJobClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationForInactiveUsersJob(IRecurringJobManager backgroundJobClient, IServiceScopeFactory scopeFactory)
        {
            _backgroundJobClient = backgroundJobClient;
            _scopeFactory = scopeFactory;
        }

        public void ScheduleNotificationJob()
        {
            _backgroundJobClient.AddOrUpdate(
                "NotifyInactiveUsers",
                () => NotifyInactiveUsers(new CancellationToken()),
                Cron.Daily);
        }

        public void NotifyInactiveUsers(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var notificationFactory = scope.ServiceProvider.GetRequiredService<NotificationFactory>();

                var notification = notificationFactory.Create(NotificationTrigger.INACTIVE_USER_REMINDER);

                var inactiveUsers = userRepository.GetInactiveUsersAsync(cancellationToken).Result;

                foreach (var user in inactiveUsers)
                {
                    notification.Prepare(user, cancellationToken);
                }
                notification.SendAsync(cancellationToken).GetAwaiter().GetResult();
            }
        }
    }
}