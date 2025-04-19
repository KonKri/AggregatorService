using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using NewsAPI;
using NewsAPI.Models;
using Polly;
using Polly.Retry;
using System.Diagnostics;

namespace AggregatorService.Infrastructure;

internal class NewsService : INewsService
{
    private readonly NewsApiClient _client;
    private readonly AsyncRetryPolicy<ArticlesResult> _retryPolicy;
    private readonly ILogger<NewsService> _logger;

    public NewsService(NewsApiClient client, ILogger<NewsService> logger)
    {
        _client = client;

        // create the policy to handle exceptions and if the status is error.
        _retryPolicy = Policy
            .Handle<Exception>()
            .OrResult<ArticlesResult>(res => res.Status == NewsAPI.Constants.Statuses.Error)
            .RetryAsync(3);
        _logger = logger;
    }

    public async Task<(List<NewsItem>, long, bool)> FetchAsync(string query, DateTime? from, DateTime? to)
    {
        // prepare req.
        var req = new EverythingRequest
        {
            Q = query,
            From = from,
            To = to
        };

        try
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // make the http call using the polly policy we created in the ctor.
            var res = await _retryPolicy.ExecuteAsync(async () =>
            {
                // make http call to news api.
                var res = await _client.GetEverythingAsync(req);

                return res;
            });

            stopWatch.Stop();

            // map results to domain model.
            return (res.Articles.Select(a => a.ToNewsItem()).ToList(), stopWatch.ElapsedMilliseconds, false);
        }
        catch (Exception ex)
        {
            _logger.LogError("NewsApi unavailable, returning fallback.");
            return (new List<NewsItem>
            {
                new NewsItem
                {
                    Title = "-",
                    Content = "-",
                    Author = "-",
                    Description = "-",
                }
            }, 0, true);
        }
    }
}
