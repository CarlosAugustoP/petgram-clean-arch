using API.Abstractions.DTOs.Moment;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using Application.Abstractions.Moments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/moments")]
    public class MomentController : PetGramController
    {
        private readonly IMediator _mediator;

        public MomentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new moment for the current user
        /// </summary>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateMoment([FromForm] CreateMomentCommand command)
        {
            command.SetAuthorId(CurrentUser.Id);
            var result = await _mediator.Send(command);
            return Ok(Result<MomentDTO>.Success(new MomentDTO().Map(result)));
        }

        /// <summary>
        /// Gets Feed
        /// </summary>
        /// <returns></returns>
        [HttpGet("feed")]
        [Authorize]
        public async Task<IActionResult> GetFeed()
        {
            var command = new GetMomentFeedQuery(CurrentUser.Id);
            var result = await _mediator.Send(command);
            return Ok(Result<List<MomentDTO>>.Success(result.Select(x => new MomentDTO().Map(x)).ToList()));
        }

        /// <summary>
        /// Gets moments by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserMoments([FromRoute] Guid userId)
        {
            var command = new GetMomentsByUserQuery(userId);
            var result = await _mediator.Send(command);
            return Ok(Result<List<MomentDTO>>.Success(result.Select(x => new MomentDTO().Map(x)).ToList()));
        }
    }
}