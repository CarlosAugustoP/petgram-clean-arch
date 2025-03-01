using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Common;

namespace Application.Abstractions.Comments.GetCommentsFromPostQuery
{
    public sealed record GetCommentsFromPostQuery : IRequest<PaginatedList<Comment>> 
    {
        public Guid PostId { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public GetCommentsFromPostQuery(Guid postId, int pageNumber, int pageSize)
        {
            PostId = postId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    internal sealed class GetCommentsFromPostQueryHandler : IRequestHandler<GetCommentsFromPostQuery, PaginatedList<Comment>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsFromPostQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<PaginatedList<Comment>> Handle(GetCommentsFromPostQuery request, CancellationToken cancellationToken)
        {
            return await _commentRepository.GetCommentsByPostIdAsync(request.PostId, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}