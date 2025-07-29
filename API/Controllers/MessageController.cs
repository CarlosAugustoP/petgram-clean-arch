using API.Abstractions.Helpers;
using API.Abstractions.Requests;
using API.Abstractions.Result;
using Application.Abstractions.Messages;
using Application.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;

namespace API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController(IMediator mediator) : PetGramController
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Creates a new message and sends it to the specified user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> CreateMessage([FromBody] MessageRequest request)
        {
            var command = new MessageCommand(CurrentUser.Id, request.SentToId, request.Content);
            var result = await _mediator.Send(command);
            return Ok(Result<MessageDto>.Success(result));
        }

        /// <summary>
        /// Retrieves the message history between the current user and the specified user. 
        /// </summary>
        [HttpGet("chat/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetMessageHistory([FromRoute] Guid userId, [FromQuery] PageRequest pageRequest)
        {
            var query = new MessageHistoryQuery(CurrentUser.Id, userId, pageRequest.PageIndex, pageRequest.PageSize);
            var result = await _mediator.Send(query);
            return Ok(Result<PaginatedList<MessageDto>>.Success(result));
        }

        /// <summary>
        /// Retrieves the count of unread messages for the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadMessageCount()
        {
            var query = new UnreadMessageCountQuery(CurrentUser.Id);
            var result = await _mediator.Send(query);
            return Ok(Result<int>.Success(result));
        }

        /// <summary>
        /// Retrieves the list of messages sent by the current user.
        /// </summary>
        [HttpGet("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatestMessages([FromQuery] PageRequest pageRequest)
        {
            var query = new LatestMessagesQuery(CurrentUser.Id, pageRequest.PageIndex, pageRequest.PageSize);
            var result = await _mediator.Send(query);
            return Ok(Result<PaginatedList<MessageHeaderDto>>.Success(result));
        }

        /// <summary>
        /// Updates the content of a message sent by the current user.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpPatch("update/{messageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMessage([FromBody] MessageRequest request, [FromRoute] Guid messageId)
        {
            var command = new UpdateMessageCommand(request.Content, messageId, CurrentUser.Id);
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }
    }
}