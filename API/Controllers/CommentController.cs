using API.Abstractions.DTOs.Comments;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using Application.Abstractions.Comments.CreateCommentCommand;
using Application.Abstractions.Comments.CreateReplyCommand;
using Application.Abstractions.Comments.DeleteCommentCommand;
using Application.Abstractions.Comments.LikeCommentCommand;
using Application.Abstractions.Comments.UpdateCommentCommand;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : PetGramController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CommentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Replies to a comment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("reply")]
        public async Task<IActionResult> ReplyToComment([FromBody] CreateReplyCommand command)
        {
            command.SetUserId(CurrentUser.Id);
            var result = await _mediator.Send(command);
            var commentDto = new CommentDto().Map(result);
            return Created("api/Comment", Result<CommentDto>.Success(commentDto));
        }

        /// <summary>
        /// Adds a like to a comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("like/{commentId}")]
        public async Task<IActionResult> LikeComment([FromRoute] Guid commentId)
        {
            var result = await _mediator.Send(new LikeCommentCommand(commentId, CurrentUser.Id));
            var commentDto = new CommentDto().Map(result);
            return Ok(Result<CommentDto>.Success(commentDto));
        }

        /// <summary>
        /// Deletes a comment or reply
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            var result = await _mediator.Send(new DeleteCommentCommand(commentId, CurrentUser.Id));
            var commentDto = new CommentDto().Map(result);
            return Ok(Result<CommentDto>.Success(commentDto));
        }

        /// <summary>
        /// Edits a comment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("{commentId}")]
        public async Task<IActionResult> EditComment([FromBody] UpdateCommentCommand command)
        {
            command.SetUserId(CurrentUser.Id);
            var result = await _mediator.Send(command);
            var commentDto = new CommentDto().Map(result);
            return Ok(Result<CommentDto>.Success(commentDto));
        }
    }
}