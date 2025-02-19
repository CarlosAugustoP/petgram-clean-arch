using API.Abstractions.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Abstractions.Helpers
{
    public class PetGramController : ControllerBase
    {
        public new UserDto CurrentUser => (UserDto)HttpContext.Items["User"];
    }
}
