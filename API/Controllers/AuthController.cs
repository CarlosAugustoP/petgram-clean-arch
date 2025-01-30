using Application.Abstractions.Users.Login;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;

        public AuthController(IJwtService jwtService, IMediator mediator)
        {
            _jwtService = jwtService;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            if (result is string)
            {
                var token = _jwtService.GenerateToken((string)result, command.Email);
                return Ok(token);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
