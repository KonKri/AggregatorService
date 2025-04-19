namespace AggregatorService.Domain;

public record WeatherItem
{
    public string City { get; set; }
    public double Temp { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; }
}
