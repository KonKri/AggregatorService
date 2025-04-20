using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Diagnostics;

namespace AggregatorService.Infrastructure;

internal class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IWeatherService> _logger;
    private readonly string _apiKey;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public WeatherService(HttpClient httpClient, ILogger<IWeatherService> logger, string apiKey)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = apiKey;

        _retryPolicy = Policy
            .Handle<Exception>()
            .OrResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
            .RetryAsync(3);
    }

    public async Task<(WeatherItem, long, bool)> FetchAsync(string city)
    {
        try
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var res = await _retryPolicy.ExecuteAsync(async () =>
            {
                var res = await _httpClient.GetAsync($"/data/2.5/weather?q={city}&appid={_apiKey}&units=metric");
                return res;
            });

            stopWatch.Stop();

            var openWeatherRes = JsonConvert.DeserializeObject<OpenWeatherResponse>(await res.Content.ReadAsStringAsync());
            return (new WeatherItem
            {
                City = openWeatherRes.CityName,
                Description = openWeatherRes.Weather.FirstOrDefault().Description,
                Humidity = openWeatherRes.Main.Humidity,
                Temp = openWeatherRes.Main.Temp
            }, stopWatch.ElapsedMilliseconds, false);
        }
        catch (Exception e)
        {
            _logger.LogError("WeatherApi unavailable, returning fallback.");
            return (new WeatherItem
            {
                City = "-",
                Description = "-",
                Humidity = -1,
                Temp = -1
            }, 0, true);
        }
    }
}
