using System.Security.Claims;
using API.Abstractions.DTOs.User;
using AutoMapper;
using Domain.CustomExceptions;
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

                    var user = await userRepository.GetByIdAsync(Guid.Parse(userId), CancellationToken.None);
                    if (user != null)
                    {
                        if (user.LastLogin != DateTime.UtcNow.Date)
                        {
                            // Update last login date
                            user.SetLastLogin(DateTime.UtcNow.Date);
                            if (!user.IsActive()) user.ActivateUser();
                            await userRepository.UpdateUserAsync(user, CancellationToken.None);
                        }
                        
                        if (user.IsBanned())
                        {
                            throw new ForbiddenException("You are currently banned from using PetGram.");
                        }

                        if (user.IsArchived())
                        {
                            throw new ForbiddenException("Your account is archived");
                        }
                        if (user.IsDeleted())
                        {
                            throw new ForbiddenException("Your account has been permanently deleted");
                        }
                        context.Items["User"] = _mapper.Map<UserDto>(user);
                    }
                }
            }
            await _next(context);
        }
    }
}
