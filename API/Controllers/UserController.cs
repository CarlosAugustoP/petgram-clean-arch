﻿using Application.Abstractions.Followers.StartFollowing;
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
namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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
        /// <param name="command"></param>
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
            if (result is string)
            {
                return Ok(Result<string>.Success(result.ToString()!));
            }else if (result is Dictionary<string,string>)
            { 
                return Created("api/User/signup", Result<Dictionary<string,string>>.Success(
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
            return Ok(followers.Items.Select(f => _mapper.Map<UserDto>(f)));
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
            return Ok(following.Items.Select(f => _mapper.Map<UserDto>(f)));
        }
    
    }
}

