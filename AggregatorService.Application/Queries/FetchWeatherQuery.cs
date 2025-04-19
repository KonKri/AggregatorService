using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class FetchWeatherQuery : IRequest<WeatherItem>
{
    public string WeatherCity { get; set; }
}
