using AggregatorService.Application.Queries;

namespace AggregatorService.Application.Extensions;

public static class RequestExtensions
{
    public static FetchNewsQuery ToFetchNewsQuery(this AggregateQuery q) => new FetchNewsQuery
    {
        NewsFrom = q.NewsFrom,
        NewsTo = q.NewsTo,
        NewsQuery = q.NewsQuery,
    };

    public static FetchWeatherQuery ToFetchWeatherQuery(this AggregateQuery q) => new FetchWeatherQuery
    {
        WeatherCity = q.WeatherCity,
    };

    public static FetchGithubUserQuery ToFetchGithubUserQuery(this AggregateQuery q) => new FetchGithubUserQuery
    {
        Username = q.GithubUser,
    };
}
