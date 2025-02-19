using System.Security.Claims;
using API.Abstractions.DTOs;
using AutoMapper;
using Domain.Repositorys;

namespace API.Middlewares
{
    public class UserValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMapper _mapper;

        public UserValidationMiddleware(RequestDelegate next, IMapper mapper)
        {
            _next = next;
            _mapper = mapper;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {

                    var user = await userRepository.GetByIdAsync(Guid.Parse(userId));
                    
                    if (user != null) 
                        context.Items["User"] = _mapper.Map<UserDto>(user);
                }
            }
            await _next(context);
        }
    }
}
