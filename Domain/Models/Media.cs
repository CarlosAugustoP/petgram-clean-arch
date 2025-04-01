using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Media
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public List<Pet> MentionedPets { get; set; } = new List<Pet>();
        public string? Title { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Media() { }

        public Media(Guid id, Guid postId, Post post, string? title, string url, string type, string? description, DateTime createdAt, List<Pet> mentionedPets)
        {
            Id = id;
            PostId = postId;
            Post = post;
            Title = title;
            Url = url;
            Type = type;
            Description = description;
            CreatedAt = createdAt;
            MentionedPets = mentionedPets ?? new List<Pet>();
        }
    }
}
