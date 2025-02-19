using AutoMapper;
using Domain.Models;
namespace API.Abstractions.DTOs
{
    [AutoMap(typeof(User))]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Name { get; set; }
    }
}
