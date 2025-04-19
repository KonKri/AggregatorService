using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class AggregateQuery : IRequest<Aggregate>
{
    public DateTime? NewsFrom { get; set; }
    public DateTime? NewsTo { get; set; }
    public string NewsQuery { get; set; }
}
