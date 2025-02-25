using System.Runtime.Intrinsics.Arm;
using API.Abstractions.DTOs.Media;
using Domain.Models;

namespace API.Abstractions.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title {  get; set; }
        public List<MediaDTO> Medias { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int Shares { get; set; }

        public PostDto()
        {
        }	
        public PostDto(Guid id, Guid authorId, string title, string content, int likes, int shares, List<MediaDTO> medias, int comments)
        {
            Id = id;
            AuthorId = authorId;
            Title = title;
            Content = content;
            Likes = likes;
            Shares = shares;
            Medias = medias;
            Comments = comments;
        }

        public PostDto Map(Post post)
        {
            var mediaList = new List<MediaDTO>(); 
            foreach (var media in post.Medias){
                mediaList.Add(new MediaDTO(media.Id, media.Url, media.Type));
            }
            return new PostDto(post.Id, post.AuthorId, post.Title, post.Content,
                post.LikesCount, post.Shares, mediaList, post.CommentsCount);
        }
    }
}