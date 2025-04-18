using AggregatorService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Api.Controllers;

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
    public async Task<IActionResult> FetchAsync()
    {
        var query = new AggregateQuery();
        var res = await _mediator.Send(query);
        return Ok(res);
    }
}
