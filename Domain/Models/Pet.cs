using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Pet
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string? Breed { get; set; }
        public string Species { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? BirthDate { get; set; }
        public int CuteMeter { get; set; }
        public List<Media> Medias { get; set; } = new List<Media>();
        public Pet()
        {
        }

        public Pet(Guid id, Guid ownerId, User owner, string name, string imgUrl, string? breed, string species, string? description, DateTime createdAt, DateTime? birthDate, int cuteMeter, List<Media>? medias)
        {
            Id = id;
            OwnerId = ownerId;
            Owner = owner;
            Name = name;
            ImgUrl = imgUrl;
            Breed = breed;
            Species = species;
            Description = description;
            CreatedAt = createdAt;
            BirthDate = birthDate;
            CuteMeter = cuteMeter;
            Medias = medias ?? new List<Media>();
        }

        
    }
}
