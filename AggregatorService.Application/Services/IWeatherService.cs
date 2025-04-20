using AggregatorService.Domain;

namespace AggregatorService.Application.Services;

public interface IWeatherService
{
    Task<(WeatherItem, long, bool)> FetchAsync(string city);
}