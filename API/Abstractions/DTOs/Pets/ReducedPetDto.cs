using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace API.Abstractions.DTOs
{
    [AutoMap(typeof(Domain.Models.Pet))]
    public class ReducedPetDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string ImgUrl { get; set; }
        public required string Species { get; set; }

        [SetsRequiredMembers]
        public ReducedPetDto(Guid id, string name, string imgUrl, string species)
        {
            Id = id;
            Name = name;
            ImgUrl = imgUrl;
            Species = species;
        }
    }
}