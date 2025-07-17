using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models.UserAggregate;

namespace API.Abstractions.DTOs.User
{

    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; } = UserRole.COMMON;
        public string? Bio { get; set; }
        public int FollowedCount { get; set; }
        public int FollowersCount { get; set; }

        //ctor 
        public UserProfileDto(Guid id, string profileImgUrl, string name, UserRole role = UserRole.COMMON, string? bio = null, int followedCount = 0, int followersCount = 0)
        {
            Id = id;
            ProfileImgUrl = profileImgUrl;
            Name = name;
            Role = role;
            Bio = bio;
            FollowedCount = followedCount;
            FollowersCount = followersCount;
        }
    }
}