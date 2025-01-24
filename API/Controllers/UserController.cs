using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Followers.StartFollowing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
    
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("follow")]
        public IActionResult UserFollowUser([FromBody] StartFollowingCommand command)
        {
            return Ok(_mediator.Send(command));
        }

    }
}
