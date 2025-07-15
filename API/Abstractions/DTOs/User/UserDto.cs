using AutoMapper;
using Domain.Models;
namespace API.Abstractions.DTOs.User
{
    [AutoMap(typeof(Domain.Models.UserAggregate.User))]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Name { get; set; }

        public UserDto(Guid id, string profileImgUrl, string name)
        {
            Id = id;
            ProfileImgUrl = profileImgUrl;
            Name = name;
        }

        public UserDto()
        {
        }

        
    }
}
