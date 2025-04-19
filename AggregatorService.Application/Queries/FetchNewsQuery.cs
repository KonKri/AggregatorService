using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class FetchNewsQuery : IRequest<List<NewsItem>>
{
    public DateTime? NewsFrom { get; set; }
    public DateTime? NewsTo { get; set; }
    public string NewsQuery { get; set; }
}
