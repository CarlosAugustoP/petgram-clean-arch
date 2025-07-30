using Application.Abstractions.Followers.StartFollowing;
using Application.Abstractions.Users.AddNewUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Abstractions.Result;
using Microsoft.AspNetCore.Authorization;
using API.Abstractions.Helpers;
using Application.Abstractions.Followers.GetFollowers;
using Application.Abstractions.Followers.GetFollowingByUser;
using API.Abstractions.Requests;
using API.Abstractions.DTOs.User;
using Microsoft.AspNetCore.RateLimiting;
using Application.Abstractions.Users.Passwords;
using Domain.Models.UserAggregate;
using API.Middlewares;
using Application.Abstractions.Users.BanUser;
using Application.Abstractions.Users.GetProfile;
using Application.Abstractions.Users.ArchiveOrDeleteProfile;
using Application.Abstractions.Users.UpdateUser;
using SharedKernel.Common;
using API.Abstractions.DTOs.Reports;
namespace API.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UserController : PetGramController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Follows a user
        /// </summary>
        /// <param name="followedId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("follow/{followedId}")]
        public async Task<IActionResult> UserFollowUser([FromRoute] Guid followedId)
        {
            var result = await _mediator.Send(new StartFollowingCommand
            {
                FollowerId = CurrentUser.Id,
                FollowedId = followedId
            });
            var userDto = _mapper.Map<UserDto>(result);
            return Ok(userDto);
        }

        /// <summary>
        /// Creates a new user, inserting it into the database
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] AddNewUserCommand command)
        {
            var result = await _mediator.Send(command);
            //Returns a string saying user already made 
            if (result is string)
            {
                return Ok(Result<string>.Success(result.ToString()!));
            }
            //Returns userKey : userguid so the fe knows what key to send when validate
            else if (result is Dictionary<string, string>)
            {
                return Created("api/User/signup", Result<Dictionary<string, string>>.Success(
                    (Dictionary<string, string>)result));
            }
            else return StatusCode(500, "Internal Server Error");
        }

        /// <summary>
        /// Fetches all the followers for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("followers")]
        public async Task<IActionResult> GetFollowers([FromQuery] PageRequest pageRequest)
        {
            var followers = await _mediator.Send(new GetFollowersByUserQuery
            {
                UserId = CurrentUser.Id,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize
            });
            return Ok(Result<PaginatedList<UserDto>>.Success(followers.Select(f => _mapper.Map<UserDto>(f))));
        }

        /// <summary>
        /// Fetches all the users that the current user is following
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("following")]
        public async Task<IActionResult> GetFollowing([FromQuery] PageRequest pageRequest)
        {
            var following = await _mediator.Send(new GetFollowingByUserQuery
            {
                UserId = CurrentUser.Id,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize
            });
            return Ok(Result<PaginatedList<UserDto>>.Success(following.Select(f => _mapper.Map<UserDto>(f))));
        }
        /// <summary>
        /// Requests a password change for a user by sending an email with a link to reset the password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request-password-change/{email}")]
        public async Task<IActionResult> RequestPasswordChange([FromRoute] string email)
        {
            var result = await _mediator.Send(new CallNewPasswordCommand(email));
            return Ok(Result<bool>.Success(true));
        }
        /// <summary>
        /// Resets the password for a user using an access link
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] AccessLinkCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }

        /// <summary>
        /// Bans a user from the platform
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ban-user")]
        [Admin]
        public async Task<IActionResult> BanUser([FromBody] BanUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }

        /// <summary>
        /// Gets the profile of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var command = new GetProfileQuery(CurrentUser.Id);
            var result = await _mediator.Send(command);
            var dto = new UserProfileDto
            (
                result.Item1.Id,
                result.Item1.ProfileImgUrl,
                result.Item1.Name,
                result.Item1.Role,
                result.Item1.Bio,
                result.Item2,
                result.Item3
            );
            return Ok(Result<UserProfileDto>.Success(dto));
        }

        /// <summary>
        /// Updates the profile of the current user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserCommand command)
        {
            command.SetUserId(CurrentUser.Id);
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }

        /// <summary>
        /// Requests to archive the user's profile, which will set the status to archived after a confirmation
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("request-archive-profile")]
        [Authorize]
        public async Task<IActionResult> RequestArchiveProfile([FromBody] string Password)
        {
            var command = new ArchiveOrDeleteProfileSolicitationCommand(CurrentUser.Id, UserStatus.ARCHIVED, Password);
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }

        /// <summary>
        /// Requests to delete the user's profile, which will set the status to deleted after a confirmation
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("request-delete-profile")]
        [Authorize]
        public async Task<IActionResult> RequestDeleteProfile([FromBody] string Password)
        {
            var command = new ArchiveOrDeleteProfileSolicitationCommand(CurrentUser.Id, UserStatus.DELETED, Password);
            var result = await _mediator.Send(command);
            return Ok(Result<bool>.Success(result));
        }

        /// <summary>
        /// Reports a user for inappropriate behavior or content
        /// </summary>
        /// <param name="reportedId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("report/{reportedId}")]
        [Authorize]
        public async Task<IActionResult> ReportUser([FromRoute] Guid reportedId, [FromBody] ReportUserRequest request)
        {
            var command = new Application.Abstractions.Reports.ReportUserCommand(
                ReporterId: CurrentUser.Id,
                ReportedId: reportedId,
                ReasonText: request.ReasonText,
                ReasonType: request.ReasonType,
                PostIds: request.PostIds,
                MomentIds: request.MomentIds
            );
            var result = await _mediator.Send(command);
            return Ok(Result<ReportDto>.Success(_mapper.Map<ReportDto>(result)));
        }
    }
}

