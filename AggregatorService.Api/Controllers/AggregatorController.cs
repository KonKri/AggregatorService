using AggregatorService.Api.Extensions;
using AggregatorService.Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AggregatorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AggregatorController> _logger;

    public AggregatorController(IMediator mediator, ILogger<AggregatorController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> FetchAsync([FromQuery] FetchRequest request)
    {
        var query = request.ToAggregateQuery();

        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> FetchStatisticsAsync()
    {

        var res = await _mediator.Send(query);
        return Ok(res);
    }
}
