using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Abstractions.DTOs.User;
using AutoMapper;

namespace API.Abstractions.DTOs.Likes
{
    public class LikeDto
    {
        public Guid Id { get; set; }
        public UserDto Author { get; set; }

        public LikeDto(Guid id, UserDto author)
        {
            Id = id;
            Author = author;
        }

        public LikeDto()
        {
        }

        public LikeDto Map(Domain.Models.Like like)
        {
            return new LikeDto(
                like.Id,
                new UserDto(like.AuthorId, like.Author!.ProfileImgUrl!, like.Author.Name)
            );
        }
        
    }
}