using AggregatorService.Application.Services;
using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

internal class FetchNewsQueryHandler : IRequestHandler<FetchNewsQuery, List<NewsItem>>
{
    private readonly INewsService _newsService;
    private readonly IHttpRequestItemsRepository _repo;

    public FetchNewsQueryHandler(INewsService newsService, IHttpRequestItemsRepository repo)
    {
        _newsService = newsService;
        _repo = repo;
    }

    public async Task<List<NewsItem>> Handle(FetchNewsQuery request, CancellationToken cancellationToken)
    {
        // todo: implement caching.

        // fetch news from news service.
        var news = await _newsService.FetchAsync();

        // persist http request to repo.
        // todo: could add hangfire or sth like that for the persistance to take place later.
        await _repo.WriteAsync(ApisEnum.NewsApi, news.Item2, DateTime.UtcNow);

        return news.Item1;
    }
}
