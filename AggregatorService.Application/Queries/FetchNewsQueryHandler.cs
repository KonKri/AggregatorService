using AggregatorService.Application.Services;
using AggregatorService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AggregatorService.Application.Queries;

internal class FetchNewsQueryHandler : IRequestHandler<FetchNewsQuery, List<NewsItem>>
{
    private readonly INewsService _newsService;
    private readonly IHttpRequestItemsRepository _repo;
    private readonly ILogger<FetchNewsQueryHandler> _logger;


    public FetchNewsQueryHandler(INewsService newsService, IHttpRequestItemsRepository repo, ILogger<FetchNewsQueryHandler> logger)
    {
        _newsService = newsService;
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<NewsItem>> Handle(FetchNewsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting FetchNews handling.");
        // todo: implement caching.

        // fetch news from news service.
        var news = await _newsService.FetchAsync();
        _logger.LogInformation("News fetched.");


        // persist http request to repo.
        // todo: could add hangfire or sth like that for the persistance to take place later.
        await _repo.WriteAsync(ApisEnum.NewsApi, news.Item2, DateTime.UtcNow);
        _logger.LogInformation("HttpRequest persisted.");


        return news.Item1;
    }
}
