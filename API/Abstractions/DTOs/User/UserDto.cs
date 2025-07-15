using AutoMapper;
using Domain.Models;
using Domain.Models.UserAggregate;
namespace API.Abstractions.DTOs.User
{
    [AutoMap(typeof(Domain.Models.UserAggregate.User))]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; } = UserRole.COMMON;

        public UserDto(Guid id, string profileImgUrl, string name, UserRole role = UserRole.COMMON)
        {
            Id = id;
            ProfileImgUrl = profileImgUrl;
            Name = name;
            Role = role;
        }

        public UserDto()
        {
        }

        
    }
}
