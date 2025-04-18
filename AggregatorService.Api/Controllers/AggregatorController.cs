using AggregatorService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AggregatorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AggregatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> FetchAsync()
    {
        var query = new AggregateQuery();
        var res = await _mediator.Send(query);
        return Ok(res);
    }
}
