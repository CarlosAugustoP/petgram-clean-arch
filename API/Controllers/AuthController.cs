using API.Abstractions.Result;
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

        /// <summary>
        /// Logs in the user and returns an authentication token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            var token = _jwtService.GenerateToken(result.ToString()!, command.Email);
            return Ok(Result<string>.Success(token));
        }
    }
}
