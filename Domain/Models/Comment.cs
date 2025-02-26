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

        public Comment()
        {
        }

        public Comment(Guid id, Guid authorId, User author, string content, DateTime createdAt, List<Comment> replies, List<Like> likes)
        {
            Id = id;
            AuthorId = authorId;
            Author = author;
            Content = content;
            CreatedAt = createdAt;
            Replies = replies;
            Likes = likes;
        }
    }
}
