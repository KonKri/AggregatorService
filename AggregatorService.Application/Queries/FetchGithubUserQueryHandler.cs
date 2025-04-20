using AggregatorService.Application.Services;
using AggregatorService.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AggregatorService.Application.Queries;

internal class FetchGithubUserQueryHandler : IRequestHandler<FetchGithubUserQuery, GithubUserItem>
{
    private readonly IGithubService _githubService;
    private readonly IHttpRequestItemsRepository _repo;
    private readonly ILogger<FetchNewsQueryHandler> _logger;
    private readonly IMemoryCache _cache;

    public FetchGithubUserQueryHandler(
        IHttpRequestItemsRepository repo,
        ILogger<FetchNewsQueryHandler> logger,
        IMemoryCache cache,
        IGithubService githubService)
    {
        _repo = repo;
        _logger = logger;
        _cache = cache;
        _githubService = githubService;
    }

    public async Task<GithubUserItem> Handle(FetchGithubUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting FetchGithubUser handling.");

        return await _cache.GetOrCreateAsync<GithubUserItem>(GetCacheKeyFromRequest(request), async x =>
        {
            // keep the informatiton cached for only a short period of time.
            x.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            // fetch github user from github service.
            var githubUser = await _githubService.FetchAsync(request.Username);

            _logger.LogInformation("Github User fetched.");

            // check if the data is fallback. If they are fallback, do not write the request time.
            if (!githubUser.Item3)
            {
                // persist http request to repo.
                await _repo.WriteAsync(ApisEnum.GithubApi, githubUser.Item2, DateTime.UtcNow);
                _logger.LogInformation("HttpRequest persisted.");
            }

            return githubUser.Item1;
        });
    }

    // <summary>
    /// Returns the key that will be used to cache information based on given filters.
    /// </summary>
    private static string GetCacheKeyFromRequest(FetchGithubUserQuery request)
    {
        var builder = new StringBuilder();
        builder.AppendJoin("_", "GITHUB", request.Username);

        return builder.ToString();
    }
}
