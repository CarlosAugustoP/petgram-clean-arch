using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Abstractions;
using Application.Abstractions.Followers.StartFollowing;
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
        public IActionResult UserFollowUser([FromBody] StartFollowingCommand command)
        {
            UserDto userDto = new UserDto();
            var result = _mediator.Send(command);
            return Ok(_mapper.Map( result,userDto ));
        }

    }
}
