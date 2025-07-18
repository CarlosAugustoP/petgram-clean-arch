using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using Application.Abstractions.Notifications;
using Application.Abstractions.Notifications.GetUnreadCountQuery;
using Application.Notifications.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : PetGramController
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("by-user")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsByUser()
        {
            var result = await _mediator.Send(new GetNotificationsQuery(CurrentUser.Id));
            return Ok(Result<List<NotificationDTO>>.Success(result));
        }

        [HttpPost("mark-as-read")]
        [Authorize]
        public async Task<IActionResult> MarkNotificationsAsRead([FromBody] List<Guid> notificationIds)
        {
            var command = new MarkAsReadCommand(notificationIds, CurrentUser.Id);
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }

        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            var result = await _mediator.Send(new GetUnreadCountQuery(CurrentUser.Id));
            return Ok(Result<int>.Success(result));
        }
    }
}