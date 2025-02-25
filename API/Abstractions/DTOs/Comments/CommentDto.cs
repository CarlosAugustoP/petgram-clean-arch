using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Abstractions.DTOs.User;
using AutoMapper;

namespace API.Abstractions.DTOs.Comments
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public UserDto Author { get; set; }
        public int Likes { get; set; }
        public int Replies { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int LikesCount { get; set; }
        public int RepliesCount { get; set; }

        public CommentDto(Guid id, string content, UserDto author, int likes, int replies, DateTime createdAt, bool isEdited)
        {
            Id = id;
            Content = content;
            Author = author;
            Likes = likes;
            Replies = replies;
            CreatedAt = createdAt;
            IsEdited = isEdited;
        }
        
        public CommentDto()
        {
        }

        public CommentDto Map(Domain.Models.Comment comment)
        {
            return new CommentDto(
                comment.Id,
                comment.Content,
                new UserDto(comment.Author.Id, comment.Author.ProfileImgUrl!, comment.Author.Name),
                comment.LikeCount,
                comment.RepliesCount,
                comment.CreatedAt,
                comment.IsEdited
            );
        }
        
    }
}