using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public string Title { get; set; }
        public List<Media> Medias { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Report>? Reports {get; set;} = new List<Report>();
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int Shares { get; set; }
        public Post()
        {
            
        }

        public Post(Guid id, Guid authorId, User? author,
            string title, List<Media> medias, string content,
            List<Comment> comments, DateTime createdAt, List<Like> likes,
            int shares)
        {
            Id = id;
            AuthorId = authorId;
            Author = author;
            Title = title;
            Medias = medias;
            Content = content;
            Comments = comments;
            CreatedAt = createdAt;
            Likes = likes;
            Shares = shares;
        }
    }
}
