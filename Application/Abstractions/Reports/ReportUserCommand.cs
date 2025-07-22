using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Models;
using Domain.Models.UserAggregate;
using Domain.Repositorys;
using MediatR;

namespace Application.Abstractions.Reports
{
    public sealed record ReportUserCommand
        (Guid ReporterId, Guid ReportedId, string ReasonText, BanReason ReasonType, List<Guid>? PostIds, List<Guid>? MomentIds)
     : IRequest<Report>;

    internal sealed class ReportUserCommandHandler : IRequestHandler<ReportUserCommand, Report>
    {
        private readonly IReportUserRepository _reportRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMomentRepository _momentRepository;
        private readonly IUserRepository _userRepository;

        public ReportUserCommandHandler(IReportUserRepository reportRepository, IUserRepository userRepository, IPostRepository postRepository, IMomentRepository momentRepository)
        {
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _momentRepository = momentRepository;
        }

        public async Task<Report> Handle(ReportUserCommand request, CancellationToken cancellationToken)
        {
            List<Post> posts = new();
            List<Moment> moments = new();

            foreach (var posId in request.PostIds ?? Enumerable.Empty<Guid>())
            {
                var post = await _postRepository.GetPostByIdAsync(posId, cancellationToken)
                    ?? throw new NotFoundException($"Post with ID {posId} not found.");
                posts.Add(post);
            }
            foreach (var momentId in request.MomentIds ?? Enumerable.Empty<Guid>())
            {
                var moment = await _momentRepository.GetByIdAsync(momentId, cancellationToken)
                    ?? throw new NotFoundException($"Moment with ID {momentId} not found.");
                moments.Add(moment);
            }

            var report = new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = request.ReporterId,
                ReportedId = request.ReportedId,
                ReasonText = request.ReasonText,
                ReasonType = request.ReasonType,
                CreatedAt = DateTime.UtcNow,
                Posts = posts,
                Moments = moments
            };

            await _reportRepository.CreateAsync(report, cancellationToken);
            return report;
        }
    }
}