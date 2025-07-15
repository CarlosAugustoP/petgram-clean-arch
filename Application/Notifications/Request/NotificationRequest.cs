using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Notifications.Request
{
    public record NotificationRequest(Guid To, string Title, string Message);
}