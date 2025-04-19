using AggregatorService.Domain;

namespace AggregatorService.Application.Services;

public interface INewsService
{
    /// <summary>
    /// Fetches news items, and the time elapsed for the http call to take place.
    /// </summary>
    /// <returns></returns>
    Task<(List<NewsItem>, long)> FetchAsync(string query, DateTime? from, DateTime? to);
}