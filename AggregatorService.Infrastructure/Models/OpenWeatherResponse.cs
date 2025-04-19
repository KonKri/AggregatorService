using Newtonsoft.Json;

namespace AggregatorService.Infrastructure.Models;

internal class OpenWeatherResponse
{
    [JsonProperty("weather")]
    public List<OpenWeatherCondition> Weather { get; set; }

    [JsonProperty("main")]
    public OpenTemperatureInfo Main { get; set; }

    [JsonProperty("wind")]
    public OpenWindInfo Wind { get; set; }

    [JsonProperty("name")]
    public string CityName { get; set; }
}

internal class OpenWeatherCondition
{
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("icon")]
    public string Icon { get; set; }
}

internal class OpenTemperatureInfo
{
    [JsonProperty("temp")]
    public double Temp { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }
}

internal class OpenWindInfo
{
    [JsonProperty("speed")]
    public double Speed { get; set; }
}
