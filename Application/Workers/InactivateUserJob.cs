using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Workers
{
    public class InactivateUserJob
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InactivateUserJob(IRecurringJobManager recurringJobManager, IServiceScopeFactory serviceScopeFactory)
        {
            _recurringJobManager = recurringJobManager;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void Schedule()
        {
            _recurringJobManager.AddOrUpdate(
                "inactivate-user-job",
                () => ExecuteAsync(),
                Cron.Daily);
        }
        public async Task ExecuteAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var users = await userRepository.GetAllUsersAsync();
            var toDeactivate = users.Where(u => u.LastLogin < DateTime.UtcNow.AddDays(-30) && u.Status == UserStatus.ACTIVE).ToList();
            foreach (var user in toDeactivate)
            {
                user.InactivateUser();
                await userRepository.UpdateUserAsync(user, CancellationToken.None);
            }
        }
    }
}