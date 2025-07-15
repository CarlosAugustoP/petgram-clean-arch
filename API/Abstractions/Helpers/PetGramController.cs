using API.Abstractions.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace API.Abstractions.Helpers
{
    public class PetGramController : ControllerBase
    {
        public UserDto CurrentUser => (UserDto)HttpContext.Items["User"]!;
    }
}
