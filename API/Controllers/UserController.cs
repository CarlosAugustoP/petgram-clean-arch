using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Abstractions;
using Application.Abstractions.Followers.StartFollowing;
using Application.Abstractions.Users.AddNewUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
    
        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("follow")]
        public async Task<IActionResult> UserFollowUser([FromBody] StartFollowingCommand command)
        {
            var result = await _mediator.Send(command);
            var userDto = _mapper.Map<UserDto>(result);
            return Ok(userDto);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] AddNewUserCommand command)
        {
            var result = await _mediator.Send(command);  
            var userDto = _mapper.Map<UserDto>(result);
            return Ok(userDto);
        }

        
    }
}

