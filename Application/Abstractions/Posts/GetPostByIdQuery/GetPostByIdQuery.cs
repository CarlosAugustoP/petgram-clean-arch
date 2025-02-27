using Domain.CustomExceptions;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Posts.GetPostByIdQuery
{

    public sealed record GetPostByIdQuery(Guid Id) : IRequest<Post>;

    internal sealed class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly IPostRepository _postRepository;

        public GetPostByIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Post> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Post not found for the given id");
            return post;
        }
    }


}