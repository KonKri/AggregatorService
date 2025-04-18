using AggregatorService.Domain;

namespace AggregatorService.Application.Services;

public interface INewsService
{
    Task<List<NewsItem>> FetchAsync();
}