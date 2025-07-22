using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models.UserAggregate;

namespace API.Abstractions.DTOs.User
{
    public class UserDashboardDto
    {
        public Guid Id { get; set; }
        public UserStatus Status { get; set; }
        public required string Email { get; set; }
        public required string ProfileImgUrl { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserRole Role { get; set; } = UserRole.COMMON;
        public DateTime? LastLogin { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
    }

    public class UserDashboardProfile : Profile
    {
        public UserDashboardProfile()
        {
            CreateMap<Domain.Models.UserAggregate.User, UserDashboardDto>()
                .ForMember(dest => dest.FollowerCount, opt => opt.MapFrom(src => src.Followers != null ? src.Followers.Count : 0))
                .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Following != null ? src.Following.Count : 0));
        }
    }
}