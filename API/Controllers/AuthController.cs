using API.Abstractions.DTOs.User;
using API.Abstractions.Result;
using Application.Abstractions.Users.Login;
using Application.Abstractions.Users.ResendToken;
using Application.Abstractions.Users.ValidateToken;
using Application.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
        [EnableRateLimiting("login")]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            var token = _jwtService.GenerateToken(result.ToString()!, command.Email);
            return Ok(Result<string>.Success(token));
        }

        /// <summary>
        /// Actually creates the user, moving the object from redis to 
        /// </summary>
        /// <param name="vtc"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenCommand vtc){
            var result = await _mediator.Send(vtc);
            var userDto = _mapper.Map<UserDto>(result);
            return Ok(Result<UserDto>.Success(userDto));
       }

       [HttpPost]
       [EnableRateLimiting("resend-token")]
       [Route("resend-token/{userId}")]
       public async Task<IActionResult> ResendToken([FromRoute] Guid userId){
            return Ok(Result<Dictionary<string,string>>.Success(
                await _mediator.Send(new ResendTokenCommand(userId)))
            );
       }
    }
}
