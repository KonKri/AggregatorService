using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Diagnostics;

namespace AggregatorService.Infrastructure;

internal class GithubService : IGithubService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GithubService> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public GithubService(HttpClient httpClient, ILogger<GithubService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _retryPolicy = Policy
            .Handle<Exception>()
            .OrResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
            .RetryAsync(3);
    }

    public async Task<(GithubUserItem, long, bool)> FetchAsync(string username)
    {
        try
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var res = await _retryPolicy.ExecuteAsync(async () =>
            {
                var res = await _httpClient.GetAsync($"/users/{username}");
                return res;
            });

            stopWatch.Stop();

            var openWeatherRes = JsonConvert.DeserializeObject<GitHubUserResponse>(await res.Content.ReadAsStringAsync());
            return (new GithubUserItem
            {
                AvatarUrl = openWeatherRes.AvatarUrl,
                Bio = openWeatherRes.Bio,
                Email = openWeatherRes.Email,
                Location = openWeatherRes.Location,
                Login = openWeatherRes.Login,
                Name = openWeatherRes.Name,
                Url = openWeatherRes.Url
            }, stopWatch.ElapsedMilliseconds, false);
        }
        catch (Exception e)
        {
            _logger.LogError("GithubApi unavailable, returning fallback.");
            return (new GithubUserItem
            {
                AvatarUrl = "-",
                Bio = "-",
                Email = "-",
                Location = "-",
                Login = "-",
                Name = "-",
                Url = "-"
            }, 0, true);
        }
    }
}
