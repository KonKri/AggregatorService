namespace AggregatorService.Api.Requests;

public record FetchRequest
{
    public DateTime? NewsFrom { get; set; }
    public DateTime? NewsTo { get; set; }
    public string NewsQuery { get; set; }

    public string WeatherCity { get; set; }

    public string GithubUser { get; set; }
}
