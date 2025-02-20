using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Followers.CreatePostCommand
{
    public sealed record CreatePostCommand : IRequest<Post>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public List<string> MediaUrls { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
        public int Shares { get; set; }

        public CreatePostCommand(string title, List<string> mediaUrls, string content,
            DateTime createdAt, List<Like> likes, int shares, Guid userId)
        {
            Title = title;
            MediaUrls = mediaUrls;
            Content = content;
            CreatedAt = createdAt;
            Likes = likes;
            Shares = shares;
            UserId = userId;
        }
    }
    internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;
        public CreatePostCommandHandler()
        {
        }
        public Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            // TODO firebase service, indepent media for post creation,
            // i can send a list of medias, see if they are imsges or videos,
            // send to firebase get the url and save the post with the urls,
            // then persist them in their own table and connect to the post.
            // Also add a constructor for media 

            throw new NotImplementedException("Not implemented yet");
        }
    }
  
}
