using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Abstractions.DTOs.Reports;
using API.Abstractions.DTOs.User;
using API.Abstractions.Helpers;
using API.Abstractions.Result;
using API.Middlewares;
using Application.Abstractions.Reports;
using Application.Abstractions.Users.GetAllUsersQuery;
using AutoMapper;
using Domain.Models.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user-evaluation")]
    public class UserEvaluationController : PetGramController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserEvaluationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Admin]
        [HttpGet("reports/{userId}")]
        public async Task<IActionResult> GetReportsByUser([FromRoute] Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetReportsByUserQuery(userId, pageIndex, pageSize);
            var result = await _mediator.Send(query);
            return Ok(Result<List<ReportDto>>.Success(result.Select(_mapper.Map<ReportDto>).ToList()));
        }

        [Admin]
        [HttpGet("reports/count/{userId}")]
        public async Task<IActionResult> GetReportsCountByUser([FromRoute]Guid userId)
        {
            var reports = await _mediator.Send(new GetReportsByUserQuery(userId, 1, int.MaxValue));
            return Ok(Result<int>.Success(reports.Count));
        }

        [Admin]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            var l = result.Select(_mapper.Map<UserDashboardDto>).ToList();
            return Ok(Result<List<UserDashboardDto>>.Success(l));
        }

    }
}