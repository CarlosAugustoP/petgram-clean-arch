using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.Models;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Moments
{
    public record GetMomentFeedQuery(Guid CurrentUserId) : IRequest<List<Moment>>;

    internal sealed class GetMomentFeedQueryHandler : IRequestHandler<GetMomentFeedQuery, List<Moment>>
    {
        private readonly IMomentRepository _momentRepository;
        private readonly IRedisService _redisService;
        private readonly IUserRepository _userRepository;

        public GetMomentFeedQueryHandler(IMomentRepository momentRepository, IRedisService redisService, IUserRepository userRepository )
        {
            _userRepository = userRepository;
            _redisService = redisService;
            _momentRepository = momentRepository;
        }

        public async Task<List<Moment>> Handle(GetMomentFeedQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"momentFeed:{request.CurrentUserId}";

            var cachedMoments = await _redisService.GetObjectAsync<List<Moment>>(cacheKey);
            if (cachedMoments != null)
            {
                return cachedMoments;
            }

            var followers = await _userRepository.GetUserFollowersAsync(request.CurrentUserId, cancellationToken);

            List<Moment> allMoments = [];

            foreach (var follower in followers)
            {
                var moments = await _momentRepository.GetMomentsByUserIdAsync(follower.Id, cancellationToken);
                allMoments.AddRange(moments);
            }

            await _redisService.SetObjectAsync(cacheKey, allMoments, 60); // Cache for 60 minutes

            return allMoments;
        }
    }
}