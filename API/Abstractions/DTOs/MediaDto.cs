using AutoMapper;

namespace API.Abstractions.DTOs
{
    [AutoMap(typeof(Domain.Models.Media))]
    public class MediaDTO {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }

        public MediaDTO(Guid id, string url, string type)
        {
            Id = id;
            Url = url;
            Type = type;
        }
    }
}