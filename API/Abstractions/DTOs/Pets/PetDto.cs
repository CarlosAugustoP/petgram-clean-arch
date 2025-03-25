using AutoMapper;

namespace API.Abstractions.DTOs
{
    [AutoMap(typeof(Domain.Models.Pet))]
    public class PetDto 
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public required string Name { get; set; }
        public required string ImgUrl { get; set; }
        public string? Breed { get; set; }
        public required string Species { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}