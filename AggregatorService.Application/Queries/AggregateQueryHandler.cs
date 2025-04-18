using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class AggregateQueryHandler : IRequestHandler<AggregateQuery, Aggregate>
{
    private readonly IMediator _mediator;

    public AggregateQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Aggregate> Handle(AggregateQuery request, CancellationToken cancellationToken)
    {
        // todo: add more tasks here.
        var fetchNewsTask = _mediator.Send(new FetchNewsQuery());

        Task.WhenAll(fetchNewsTask);
        
        return new Aggregate
        {
            News = fetchNewsTask.Result
        };
    }

}
