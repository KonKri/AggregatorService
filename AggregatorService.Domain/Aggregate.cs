﻿namespace AggregatorService.Domain;

public record Aggregate
{
    public IEnumerable<NewsItem>? News { get; init; }
    public WeatherItem Weather { get; init; }
    public GithubUserItem GithubUser { get; init; }
}
