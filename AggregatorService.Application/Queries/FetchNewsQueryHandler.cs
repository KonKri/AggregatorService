using AggregatorService.Application.Services;
using AggregatorService.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AggregatorService.Application.Queries;

internal class FetchNewsQueryHandler : IRequestHandler<FetchNewsQuery, List<NewsItem>>
{
    private readonly INewsService _newsService;
    private readonly IHttpRequestItemsRepository _repo;
    private readonly ILogger<FetchNewsQueryHandler> _logger;
    private readonly IMemoryCache _cache;

    public FetchNewsQueryHandler(
        INewsService newsService,
        IHttpRequestItemsRepository repo,
        ILogger<FetchNewsQueryHandler> logger,
        IMemoryCache cache)
    {
        _newsService = newsService;
        _repo = repo;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<NewsItem>> Handle(FetchNewsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting FetchNews handling.");

        return await _cache.GetOrCreateAsync<List<NewsItem>>(GetCacheKeyFromRequest(request), async x => 
        {
            // keep the informatiton cached for only a short period of time.
            x.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            // fetch news from news service.
            var news = await _newsService.FetchAsync(request.NewsQuery, request.NewsFrom, request.NewsTo);
            _logger.LogInformation("News fetched.");

            // persist http request to repo.
            // todo: could add hangfire or sth like that for the persistance to take place later.
            await _repo.WriteAsync(ApisEnum.NewsApi, news.Item2, DateTime.UtcNow);
            _logger.LogInformation("HttpRequest persisted.");

            return news.Item1;
        }) ?? [];
    }

    /// <summary>
    /// Returns the key that will be used to cache information based on given filters.
    /// </summary>
    private static string GetCacheKeyFromRequest(FetchNewsQuery request)
    {
        var builder = new StringBuilder();
        builder.AppendJoin("_", request.NewsQuery, request.NewsFrom, request.NewsTo);

        return builder.ToString();
    }
}
