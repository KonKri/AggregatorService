using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class FetchNewsQuery : IRequest<List<NewsItem>>
{
    //todo: add params here.
}
