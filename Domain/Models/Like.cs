using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Models
{
    public class Like
    {
    public Like() { }
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public Post? Post { get; set; }
        public Guid? PostId {get; set;}
        public Comment? Comment { get; set; }
        public Guid? CommentId {get; set;}
        public DateTime CreatedAt { get; set; }
        public Like(Guid id, Guid authorId, User? author, Post? post, Guid? postId, Comment? comment, DateTime createdAt, Guid? commentId)
        {
            Id = id;
            AuthorId = authorId;
            Author = author;
            Post = post;
            PostId = postId;
            Comment = comment;
            CreatedAt = createdAt;
            CommentId = commentId;
        }

        public Like(Guid id, Guid authorId, User? author, Comment? comment, DateTime createdAt, Guid? commentId)
        {
            Id = id;
            AuthorId = authorId;
            Author = author;
            Comment = comment;
            CreatedAt = createdAt;
            CommentId = commentId;
        }
    }
}
