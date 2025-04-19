using AggregatorService.Domain;

namespace AggregatorService.Application.Services;

public interface INewsService
{


    /// <summary>
    /// Fetches news items, and the time elapsed for the http call to take place.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>List of NewsItems, the time of the execution, if the data is fallback.</returns>
    Task<(List<NewsItem>, long, bool)> FetchAsync(string query, DateTime? from, DateTime? to);
}