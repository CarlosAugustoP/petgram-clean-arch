using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Posts.CreatePostCommand
{
    public record MediaRequest(string Base64String, List<Guid> ReferencedPets);
}