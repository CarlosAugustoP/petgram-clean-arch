using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; } = false;
        public List<Comment>? Replies { get; set; } = new List<Comment>();
        public List<Like>? Likes { get; set; } = new List<Like>();
        public int LikeCount { get; set; }
        public int RepliesCount { get; set; }
        public Post? Post { get; set; }
        public Guid? PostId { get; set; }
        public Comment? BaseComment { get; set; }
        public Guid? BaseCommentId { get; set; }

        public Comment()
        {
        }
        // crie um construtor especifico para criação de um comentario num post
        public Comment(Guid id, Guid authorId, User author, string content, DateTime createdAt, Post? post, Guid postId)
        {
            Id = id;
            AuthorId = authorId;
            Author = author;
            Content = content;
            CreatedAt = createdAt;
            Post = post;
            PostId = postId;
        }

        // public Comment(Guid id, Guid authorId, User author, string content, DateTime createdAt, Post post, Guid postId)
        // {
        //     Id = id;
        //     AuthorId = authorId;
        //     Author = author;
        //     Content = content;
        //     CreatedAt = createdAt;
        //     Post = post;
        //     PostId = postId;
        // }
    }
}
