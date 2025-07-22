using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UserManagement;
using Domain.Repositorys;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Workers
{
    public class UserManageBanJob
    {
        private readonly IRecurringJobManager _backgroundJobClient;
        private readonly IServiceScopeFactory _scopeFactory;
        public UserManageBanJob(IRecurringJobManager backgroundJobClient, IServiceScopeFactory scopeFactory)
        {
            _backgroundJobClient = backgroundJobClient;
            _scopeFactory = scopeFactory;
        }

        public void ScheduleUnBanUsersJob()
        {
            _backgroundJobClient.AddOrUpdate(
                "UnBanUsers",
                () => UnBanUsers(new CancellationToken()),
                Cron.Hourly);
        }

        public void UnBanUsers(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userBanRepository = scope.ServiceProvider.GetRequiredService<IUserBanRepository>();
                var banHammer = scope.ServiceProvider.GetRequiredService<IBanHammer>();

                var bannedUsers = userBanRepository.GetExpiredBansAsync(cancellationToken).Result;

                foreach (var bannedUser in bannedUsers)
                {
                    banHammer.UnBanUserAsync(bannedUser.UserId, cancellationToken, true).GetAwaiter().GetResult();
                }
            }
        }
    }
}