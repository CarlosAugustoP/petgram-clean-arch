using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Repositorys;
using Domain.Models;
using Application.Services;

namespace Application.Abstractions.Moments
{
    public record GetMomentsByUserQuery(Guid UserId) : IRequest<List<Moment>>;
    internal sealed class GetMomentsByUserQueryHandler : IRequestHandler<GetMomentsByUserQuery, List<Moment>>
    {
        private readonly IMomentRepository _momentRepository;
        private readonly IRedisService _redisService;

        public GetMomentsByUserQueryHandler(IMomentRepository momentRepository, IRedisService redisService)
        {
            _redisService = redisService;
            _momentRepository = momentRepository;
        }

        public async Task<List<Moment>> Handle(GetMomentsByUserQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"moments:{request.UserId}";
            var cachedMoments = await _redisService.GetObjectAsync<List<Moment>>(cacheKey);
            if (cachedMoments != null)
            {
                return cachedMoments;
            }
            var toLoad = await _momentRepository.GetMomentsByUserIdAsync(request.UserId, cancellationToken);
            await _redisService.SetObjectAsync(cacheKey, toLoad, 5);
            return toLoad;
        }
    }
}