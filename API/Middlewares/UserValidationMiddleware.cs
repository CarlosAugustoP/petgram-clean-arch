using System.Security.Claims;
using API.Abstractions;
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
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);
                Console.WriteLine("MEU CU" +userId);

                if (!string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("ROLA" +userId);
                    Console.WriteLine("ROLA" +userId);
                    Console.WriteLine("ROLA" +userId);
                    Console.WriteLine("ROLA" +userId);
                    Console.WriteLine("ROLA" +userId);
                    Console.WriteLine("ROLA" +userId);

                    var user = await userRepository.GetByIdAsync(Guid.Parse(userId));
                    Console.WriteLine("PENIS IMENSO" +user.Id);
                    Console.WriteLine("PENIS IMENSO" +user.Id);
                    Console.WriteLine("PENIS IMENSO" +user.Id);
                    Console.WriteLine("PENIS IMENSO" +user.Id);
                    
                    
                    if (user != null) 
                        context.Items["User"] = _mapper.Map<UserDto>(user);
                }
            }
            await _next(context);
        }
    }
}
