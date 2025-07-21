using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Abstractions.Helpers;
using API.Middlewares;
using Application.Abstractions.Reports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user-evaluation")]
    public class UserEvaluationController : PetGramController
    {
        private readonly IMediator _mediator;

        public UserEvaluationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Admin]
        [HttpGet("reports/{userId}")]
        public async Task<IActionResult> GetReportsByUser(Guid userId, int pageIndex = 1, int pageSize = 10)
        {
            var query = new GetReportsByUserQuery(userId, pageIndex, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Admin]
        [HttpGet("reports/count/{userId}")]
        public async Task<IActionResult> GetReportsCountByUser(Guid userId)
        {
            var reports = await _mediator.Send(new GetReportsByUserQuery(userId, 1, int.MaxValue));
            return Ok(reports.TotalCount);
        }

    }
}