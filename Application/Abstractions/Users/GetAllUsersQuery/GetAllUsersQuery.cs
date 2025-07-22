using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;
using SharedKernel.Extensions;

namespace Application.Abstractions.Users.GetAllUsersQuery
{
    public class GetAllUsersQuery : IRequest<List<User>>
    {
        public int PageIndex { get; } = 1;
        public int PageSize { get; } = 10;
        public required string SearchQuery { get; set; }
        public enum SortBy { NAME, FOLLOWER_COUNT, CREATED_AT, LAST_ACTIVE, POST_COUNT }
        public enum Order { ASC, DESC }
        public SortBy Sort { get; set; } = SortBy.NAME;
        public Order SortOrder { get; set; } = Order.ASC;
        public UserStatus? StatusFilter { get; set; }
    }

    internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = await _userRepository.GetAllUsersAsync(cancellationToken);

            if (request.StatusFilter.HasValue)
            {
                query = query.Where(u => u.Status == request.StatusFilter.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(u => u.Name.Contains(request.SearchQuery, StringComparison.OrdinalIgnoreCase)
                                         || u.Email.Contains(request.SearchQuery, StringComparison.OrdinalIgnoreCase));
            }
            // Sorting logic can be added here based on request.Sort and request.SortOrder
            switch (request.Sort)
            {
                case GetAllUsersQuery.SortBy.NAME:
                    query = request.SortOrder == GetAllUsersQuery.Order.ASC
                        ? query.OrderBy(u => u.Name)
                        : query.OrderByDescending(u => u.Name);
                    break;
                case GetAllUsersQuery.SortBy.FOLLOWER_COUNT:
                    query = request.SortOrder == GetAllUsersQuery.Order.ASC
                        ? query.OrderBy(u => u.Followers!.Count)
                        : query.OrderByDescending(u => u.Followers!.Count);
                    break;
                case GetAllUsersQuery.SortBy.CREATED_AT:
                    query = request.SortOrder == GetAllUsersQuery.Order.ASC
                        ? query.OrderBy(u => u.CreatedAt)
                        : query.OrderByDescending(u => u.CreatedAt);
                    break;
                case GetAllUsersQuery.SortBy.LAST_ACTIVE:
                    query = request.SortOrder == GetAllUsersQuery.Order.ASC
                        ? query.OrderBy(u => u.LastLogin)
                        : query.OrderByDescending(u => u.LastLogin);
                    break;
                case GetAllUsersQuery.SortBy.POST_COUNT:
                    query = request.SortOrder == GetAllUsersQuery.Order.ASC
                        ? query.OrderBy(u => u.Posts!.Count)
                        : query.OrderByDescending(u => u.Posts!.Count);
                    break;
                default:
                    throw new BadRequestException("Invalid sort option");
            }

            return await query.PaginateAsync(request.PageIndex, request.PageSize);
        }
    }
}