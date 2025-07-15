using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.Notifications
{
    public interface INotification
    {
        public void Prepare(object data, CancellationToken cancellationToken);
        public Task SendAsync(CancellationToken cancellationToken);
        public async Task ExecuteAsync(object data, CancellationToken cancellationToken) {
            Prepare(data, cancellationToken);
            await SendAsync(cancellationToken);
        }
    }
}