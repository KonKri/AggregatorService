using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Infrastructure.Extensions;
using NewsAPI;
using NewsAPI.Models;

namespace AggregatorService.Infrastructure;

public class NewsService : INewsService
{
    private readonly NewsApiClient _client;

    public NewsService(NewsApiClient client)
    {
        _client = client;
    }

    public async Task<List<NewsItem>> FetchAsync()
    {
        // prepare req.
        var req = new EverythingRequest
        {
            Q = "Mitsotakis",
        };

        // make http call to news api.
        var res = await _client.GetEverythingAsync(req);

        // map results to domain model.
        return res.Articles.Select(a => a.ToNewsItem()).ToList();
    }
}
