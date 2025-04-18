using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Infrastructure.Extensions;
using NewsAPI;
using NewsAPI.Models;
using System.Diagnostics;

namespace AggregatorService.Infrastructure;

public class NewsService : INewsService
{
    private readonly NewsApiClient _client;

    public NewsService(NewsApiClient client)
    {
        _client = client;
    }

    public async Task<(List<NewsItem>, long)> FetchAsync()
    {
        // prepare req.
        var req = new EverythingRequest
        {
            Q = "Mitsotakis",
        };

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        // make http call to news api.
        var res = await _client.GetEverythingAsync(req);

        stopWatch.Stop();

        // map results to domain model.
        return (res.Articles.Select(a => a.ToNewsItem()).ToList(), stopWatch.ElapsedMilliseconds);
    }
}
