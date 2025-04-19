using AggregatorService.Api.Requests;
using AggregatorService.Application.Queries;

namespace AggregatorService.Api.Extensions;

public static class RequestExtensions
{
    public static AggregateQuery ToAggregateQuery(this FetchRequest article) => new AggregateQuery
    {
        NewsFrom = article.NewsFrom,
        NewsTo = article.NewsTo,
        NewsQuery = article.NewsQuery,
    };
}
