using AggregatorService.Application.Queries;

namespace AggregatorService.Application.Extensions;

public static class RequestExtensions
{
    public static FetchNewsQuery ToFetchNewsQuery(this AggregateQuery article) => new FetchNewsQuery
    {
        NewsFrom = article.NewsFrom,
        NewsTo = article.NewsTo,
        NewsQuery = article.NewsQuery,
    };
}
