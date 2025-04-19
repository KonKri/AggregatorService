using AggregatorService.Application.Services;
using AggregatorService.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AggregatorService.Application.Queries;

public class FetchWeatherQueryHandler : IRequestHandler<FetchWeatherQuery, WeatherItem>
{
    private readonly ILogger<FetchWeatherQueryHandler> _logger;
    private readonly IMemoryCache _cache;
    private readonly IHttpRequestItemsRepository _repo;
    private readonly IWeatherService _weatherService;

    public FetchWeatherQueryHandler(
        ILogger<FetchWeatherQueryHandler> logger,
        IMemoryCache cache,
        IHttpRequestItemsRepository repo,
        IWeatherService weatherService)
    {
        _logger = logger;
        _cache = cache;
        _repo = repo;
        _weatherService = weatherService;
    }

    public async Task<WeatherItem> Handle(FetchWeatherQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting FetchWeather handling.");

        return await _cache.GetOrCreateAsync<WeatherItem>(GetCacheKeyFromRequest(request), async x =>
        {
            // keep the informatiton cached for only a short period of time.
            x.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            // fetch weather from weather service.
            var weather = await _weatherService.FetchAsync(request.WeatherCity);

            _logger.LogInformation("Weather fetched.");

            // check if the data is fallback. If they are fallback, do not write the request time.
            if (!weather.Item3)
            {
                // persist http request to repo.
                await _repo.WriteAsync(ApisEnum.OpenWeatherMap, weather.Item2, DateTime.UtcNow);
                _logger.LogInformation("HttpRequest persisted.");
            }

            return weather.Item1;
        });

    }

    // <summary>
    /// Returns the key that will be used to cache information based on given filters.
    /// </summary>
    private static string GetCacheKeyFromRequest(FetchWeatherQuery request)
    {
        var builder = new StringBuilder();
        builder.AppendJoin("_", "WEATHER", request.WeatherCity);

        return builder.ToString();
    }
}
