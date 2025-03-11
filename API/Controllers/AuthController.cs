using API.Abstractions.DTOs.User;
using API.Abstractions.Result;
using Application.Abstractions.Users.Login;
using Application.Abstractions.Users.ValidateToken;
using Application.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthController(IJwtService jwtService, IMediator mediator, IMapper mapper)
        {
            _jwtService = jwtService;
            _mediator = mediator;
            _mapper = mapper;
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

        [HttpPost]
        [Route("validate/{userId}/{email}")]
        public async Task<IActionResult> ValidateToken([FromRoute] Guid userId, [FromRoute] string email, [FromBody] string token){
            var result = await _mediator.Send(new ValidateTokenCommand(email,token, userId));
            var userDto = _mapper.Map<UserDto>(result);
            return Ok(Result<UserDto>.Success(userDto));
       }
    }
}
