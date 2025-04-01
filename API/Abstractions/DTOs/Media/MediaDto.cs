using AutoMapper;

namespace API.Abstractions.DTOs.Media
{
    [AutoMap(typeof(Domain.Models.Media))]
    public class MediaDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public List<ReducedPetDto> Pets { get; set; }


        public MediaDTO(Guid id, string url, string type, List<ReducedPetDto> pets)
        {
            Id = id;
            Url = url;
            Type = type;
            Pets = pets;
        }
    }
}