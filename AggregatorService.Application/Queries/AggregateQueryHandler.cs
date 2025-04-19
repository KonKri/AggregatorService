using AggregatorService.Application.Extensions;
using AggregatorService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AggregatorService.Application.Queries;

public class AggregateQueryHandler : IRequestHandler<AggregateQuery, Aggregate>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AggregateQueryHandler> _logger;

    public AggregateQueryHandler(IMediator mediator, ILogger<AggregateQueryHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Aggregate> Handle(AggregateQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Aggregate handling.");

        // todo: add more tasks here.
        var fetchNewsQuery = request.ToFetchNewsQuery();
        var fetchNewsTask = _mediator.Send(fetchNewsQuery);

        var fetchWeatherQuery = request.ToFetchWeatherQuery();
        var fetchWeatherTask = _mediator.Send(fetchWeatherQuery);

        await Task.WhenAll(fetchNewsTask);
        
        return new Aggregate
        {
            News = fetchNewsTask.Result,
            Weather = fetchWeatherTask.Result,
        };
    }

}
