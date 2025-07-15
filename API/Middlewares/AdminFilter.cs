using System;
using API.Abstractions.DTOs.User;
using Domain.CustomExceptions;
using Domain.Models.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Admin : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Items["User"] as UserDto;
            if (user!.Role != UserRole.ADMINISTRATOR)
            {
               throw new ForbiddenException("You do not have permission to perform this action.");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}